import { Injectable } from '@angular/core';
import { User, UserManager } from 'oidc-client';
import { BehaviorSubject, concat, from, Observable } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';
import { ApplicationPaths, ApplicationName, AuthRoles } from './api-authorization.constants';

export type IAuthenticationResult =
  SuccessAuthenticationResult |
  FailureAuthenticationResult |
  RedirectAuthenticationResult;

export interface SuccessAuthenticationResult {
  status: AuthenticationResultStatus.Success;
  state: any;
}

export interface FailureAuthenticationResult {
  status: AuthenticationResultStatus.Fail;
  message: string;
}

export interface RedirectAuthenticationResult {
  status: AuthenticationResultStatus.Redirect;
}

export enum AuthenticationResultStatus {
  Success,
  Redirect,
  Fail
}

export interface IUser {
  name?: string;
  role?: string[];
  store?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  private userManager: UserManager;
  private userSubject: BehaviorSubject<IUser | null> = new BehaviorSubject(null);

  public isAuthenticated(): Observable<boolean> {
    return this.getUser().pipe(map(u => !!u));
  }

  public isAdmin(): Observable<boolean> {
    return this.getUser().pipe(map(u => u && u.role && u.role.includes(AuthRoles.Admin)));
  }

  public isManager(): Observable<boolean> {
    return this.getUser().pipe(map(u =>
      u && u.role && u.store && u.role.includes(AuthRoles.Manager)
    ));
  }

  public isCashier(): Observable<boolean> {
    return this.getUser().pipe(map(u =>
      u && u.role && u.store && u.role.includes(AuthRoles.Cashier))
    );
  }

  public getUser(): Observable<IUser | null> {
    return concat(
      this.userSubject.pipe(take(1), filter(u => !!u)),
      this.getUserFromStorage().pipe(filter(u => !!u), tap(u => this.userSubject.next(u))),
      this.userSubject.asObservable()
    );
  }

  public getAccessToken(): Observable<string> {
    return from(this.ensureUserManagerInitialized())
      .pipe(mergeMap(() => from(this.userManager.getUser())),
        map(user => user && user.access_token)
      );
  }

  public async signIn(state: any): Promise<IAuthenticationResult> {
    await this.ensureUserManagerInitialized();
    let user: User = null;
    try {
      user = await this.userManager.signinSilent(this.createArguments());
      this.userSubject.next(user.profile);
      return this.success(state);
    } catch (silentError) {
      console.log('Silent authentication error: ', silentError);
      try {
        await this.userManager.signinRedirect(this.createArguments(state));
        return this.redirect();
      } catch (redirectError) {
        console.error('Redirect authentication error: ', redirectError);
        return this.error(redirectError);
      }
    }
  }

  public async completeSignIn(url: string): Promise<IAuthenticationResult> {
    try {
      await this.ensureUserManagerInitialized();
      const user = await this.userManager.signinCallback(url);
      this.userSubject.next(user && user.profile);
      return this.success(user && user.state);
    } catch (error) {
      console.error('There was an error signing in: ', error);
      return this.error('There was an error signing in.');
    }
  }

  public async signOut(state: any): Promise<IAuthenticationResult> {
    try {
      await this.userManager.signoutRedirect(this.createArguments(state));
      return this.redirect();
    } catch (redirectSignOutError) {
      console.error('Redirect signout error: ', redirectSignOutError);
      return this.error(redirectSignOutError);
    }
  }

  public async completeSignOut(url: string): Promise<IAuthenticationResult> {
    await this.ensureUserManagerInitialized();
    try {
      const response = await this.userManager.signoutCallback(url);
      this.userSubject.next(null);
      return this.success(response && response.state);
    } catch (error) {
      console.error(`There was an error trying to log out '${error}'.`);
      return this.error(error);
    }
  }

  private createArguments(state?: any): any {
    return { useReplaceToNavigate: true, data: state };
  }

  private error(message: string): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Fail, message };
  }

  private success(state: any): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Success, state };
  }

  private redirect(): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Redirect };
  }

  private async ensureUserManagerInitialized(): Promise<void> {
    if (this.userManager !== undefined) {
      return;
    }

    const response = await fetch(ApplicationPaths.ApiAuthorizationClientConfigurationUrl);
    if (!response.ok) {
      throw new Error(`Could not load settings for '${ApplicationName}'`);
    }

    const settings: any = await response.json();
    settings.automaticSilentRenew = true;
    settings.includeIdTokenInSilentRenew = true;
    this.userManager = new UserManager(settings);

    this.userManager.events.addUserSignedOut(async () => {
      await this.userManager.removeUser();
      this.userSubject.next(null);
    });
  }

  private getUserFromStorage(): Observable<IUser> {
    return from(this.ensureUserManagerInitialized())
      .pipe(
        mergeMap(() => this.userManager.getUser()),
        map(u => u && u.profile)
      );
  }
}
