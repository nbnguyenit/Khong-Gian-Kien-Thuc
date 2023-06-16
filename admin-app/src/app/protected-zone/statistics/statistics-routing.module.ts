import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../shared';
import { MonthlyCommentsComponent } from './monthly-comments/monthly-comments.component';
import { MonthlyKbsComponent } from './monthly-kbs/monthly-kbs.component';
import { MonthlyMembersComponent } from './monthly-members/monthly-members.component';


const routes: Routes = [
    {
        path: '',
        component: MonthlyKbsComponent
    },
    {
        path: 'monthly-newkbs',
        component: MonthlyKbsComponent,
        data: {
            functionCode: 'STATISTIC_MONTHLY_NEWKB'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'monthly-registers',
        component: MonthlyMembersComponent,
        data: {
            functionCode: 'STATISTIC_MONTHLY_NEWMEMBER'
        },
        canActivate: [AuthGuard]
    },
    {
        path: 'monthly-comments',
        component: MonthlyCommentsComponent,
        data: {
            functionCode: 'STATISTIC_MONTHLY_COMMENT'
        },
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class StatisticsRoutingModule {}
