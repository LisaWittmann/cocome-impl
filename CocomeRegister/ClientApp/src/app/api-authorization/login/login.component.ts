import { Component, OnInit } from '@angular/core';
import { AuthorizeService, AuthenticationResultStatus } from '../authorize.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { LoginActions, QueryParameterNames, ApplicationPaths, ReturnUrlType } from '../api-authorization.constants';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public message = new BehaviorSubject<string>(null);

  constructor(
    private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  async ngOnInit() {
    const action = this.activatedRoute.snapshot.url[1];
    switch (action.path) {
      case LoginActions.Login:
        await this.login(this.getReturnUrl());
        break;
      case LoginActions.LoginCallback:
        await this.processLoginCallback();
        break;
      case LoginActions.LoginFailed:
        const message = this.activatedRoute.snapshot.queryParamMap.get(QueryParameterNames.Message);
        this.message.next(message);
        break;
      case LoginActions.Profile:
        this.redirectToProfile();
        break;
      case LoginActions.Register:
        this.redirectToRegister();
        break;
      default:
        throw new Error(`Invalid action '${action}'`);
    }
  }


  private async login(returnUrl: string): Promise<void> {
    const state: INavigationState = { returnUrl };
    const result = await this.authorizeService.signIn(state);
    this.message.next(undefined);
    switch (result.status) {
      case AuthenticationResultStatus.Redirect:
        break;
      case AuthenticationResultStatus.Success:
        await this.navigateToReturnUrl(returnUrl);
        break;
      case AuthenticationResultStatus.Fail:
        await this.router.navigate(ApplicationPaths.LoginFailedPathComponents, {
          queryParams: { [QueryParameterNames.Message]: result.message }
        });
        break;
      default:
        throw new Error(`Invalid status result ${(result as any).status}.`);
    }
  }

  private async processLoginCallback(): Promise<void> {
    const url = window.location.href;
    const result = await this.authorizeService.completeSignIn(url);
    switch (result.status) {
      case AuthenticationResultStatus.Redirect:
        throw new Error('Should not redirect.');
      case AuthenticationResultStatus.Success:
        await this.navigateToReturnUrl(this.getReturnUrl(result.state));
        break;
      case AuthenticationResultStatus.Fail:
        this.message.next(result.message);
        break;
    }
  }

  private redirectToRegister(): any {
    this.redirectToApiAuthorizationPath(
      `${ApplicationPaths.IdentityRegisterPath}?returnUrl=${encodeURI('/' + ApplicationPaths.Login)}`);
  }

  private redirectToProfile(): void {
    this.redirectToApiAuthorizationPath(ApplicationPaths.IdentityManagePath);
  }

  private async navigateToReturnUrl(returnUrl: string) {
    await this.router.navigateByUrl(returnUrl, {
      replaceUrl: true
    });
  }

  private getReturnUrl(state?: INavigationState): string {
    const fromQuery = (this.activatedRoute.snapshot.queryParams as INavigationState).returnUrl;
    if (fromQuery &&
      !(fromQuery.startsWith(`${window.location.origin}/`) ||
        /\/[^\/].*/.test(fromQuery))) {
      throw new Error('Invalid return url. The return url needs to have the same origin as the current page.');
    }
    return (state && state.returnUrl) ||
      fromQuery ||
      ApplicationPaths.DefaultLoginRedirectPath;
  }

  private redirectToApiAuthorizationPath(apiAuthorizationPath: string) {
    const redirectUrl = `${window.location.origin}/${apiAuthorizationPath}`;
    window.location.replace(redirectUrl);
  }
}

interface INavigationState {
  [ReturnUrlType]: string;
}
