import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';

@NgModule({
    imports: [ServerModule, ModuleMapLoaderModule],
    bootstrap: []
})
export class StoreServerModule { }