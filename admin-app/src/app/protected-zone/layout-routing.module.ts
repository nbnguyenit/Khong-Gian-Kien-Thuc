import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../shared';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'prefix' },
            {
                path: 'dashboard',
                loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                data: {
                    functionCode: 'DASHBOARD'
                },
                canActivate: [AuthGuard]
            },
            {
                path: 'contents',
                loadChildren: () => import('./contents/contents.module').then((m) => m.ContentsModule),
                data: {
                    functionCode: 'CONTENT'
                },
                canActivate: [AuthGuard]
            },
            {
                path: 'statistics',
                loadChildren: () => import('./statistics/statistics.module').then((m) => m.StatisticsModule),
                data: {
                    functionCode: 'STATISTIC'
                },
                canActivate: [AuthGuard]
            },
            {
                path: 'systems',
                loadChildren: () => import('./systems/systems.module').then((m) => m.SystemsModule),
                data: {
                    functionCode: 'SYSTEM'
                },
                canActivate: [AuthGuard]
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule {}
