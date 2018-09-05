import { ErrorHandler, Inject, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
export class AppErrorHandler implements ErrorHandler {
   
    constructor() { }
    handleError(error: any): void {
        console.log('global error cached');
        console.log(error);
    }
}