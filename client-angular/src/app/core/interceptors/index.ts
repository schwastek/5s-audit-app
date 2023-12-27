import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { JwtInterceptor } from "./jwt.interceptor";

// Angular executes interceptors in the order in which they are provided.
// If an interceptor is listed last, it will be executed after other interceptors.
export const httpInterceptorProviders = [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
];
