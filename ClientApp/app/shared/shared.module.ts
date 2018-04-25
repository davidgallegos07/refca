import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastyModule } from 'ng2-toasty';
import { LoadingSpinnerComponent } from '../shared/loading-spinner/loading-spinner.component'
@NgModule({
    declarations: [
        LoadingSpinnerComponent
    ],

    imports: [
        CommonModule,
        FormsModule,
        //ToastyModule.forRoot()
    ],

    exports: [
        CommonModule,
        FormsModule,
        //ToastyModule,
        LoadingSpinnerComponent
    ]

})

export class SharedModule { }