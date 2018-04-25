import { Injectable, Inject, NgZone } from '@angular/core';
import { ToastyService } from "ng2-toasty";

@Injectable()
export class ToastNotificationService {
    constructor(
        //private ngZone: NgZone,
        private toastyService: ToastyService) { }

    errorMessage() {
        //this.ngZone.run(() => {
            return this.toastyService.error({
                title: 'Error',
                msg: 'No se encontro el elemento solicitado.',
                theme: 'material',
                showClose: true,
                timeout: 5000
            });
        //});
    }
}