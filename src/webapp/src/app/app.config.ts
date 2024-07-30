import {APP_INITIALIZER, ApplicationConfig, DEFAULT_CURRENCY_CODE} from "@angular/core";
import {PreloadAllModules, provideRouter, withPreloading} from "@angular/router";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import {tokenInterceptor} from "./account/user/services";
import {AppInitializerService} from "./common/app-initializer.service";
import {routes} from "./app.routes";

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([tokenInterceptor])),
    AppInitializerService,
    {
      provide: APP_INITIALIZER,
      useFactory: (appInitializer: AppInitializerService) => () => appInitializer.initializeApp(),
      deps: [AppInitializerService],
      multi: true
    },
    provideRouter(routes, withPreloading(PreloadAllModules)),
    {
      provide: DEFAULT_CURRENCY_CODE, useValue: ""
    }
  ]
};
