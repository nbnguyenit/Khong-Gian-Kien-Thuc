import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonthlyMembersComponent } from './monthly-members/monthly-members.component';
import { MonthlyKbsComponent } from './monthly-kbs/monthly-kbs.component';
import { MonthlyCommentsComponent } from './monthly-comments/monthly-comments.component';
import { StatisticsRoutingModule } from './statistics-routing.module';

import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { BlockUIModule } from 'primeng/blockui';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [MonthlyMembersComponent, MonthlyKbsComponent, MonthlyCommentsComponent],
  imports: [
    CommonModule,
    FormsModule,
    StatisticsRoutingModule,
    PanelModule,
    ButtonModule,
    TableModule,
    BlockUIModule,
    InputTextModule,
    ProgressSpinnerModule
  ]
})
export class StatisticsModule { }
