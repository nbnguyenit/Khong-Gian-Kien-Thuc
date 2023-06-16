import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from './base.service';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { MonthlyComment, MonthlyKb, MonthlyMember } from '../models';


@Injectable({ providedIn: 'root' })
export class StatisticsService extends BaseService {
    private _sharedHeaders = new HttpHeaders();
    constructor(private http: HttpClient) {
        super();
        this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
    }
    getMonthlyNewComments(year: number) {
        return this.http.get<MonthlyComment[]>(`${environment.apiUrl}/api/statistics/monthly-comments?year=${year}`,
            { headers: this._sharedHeaders })
            .pipe(map((response: MonthlyComment[]) => {
                return response;
            }), catchError(this.handleError));
    }

    getMonthlyNewKbs(year: number) {
        return this.http.get<MonthlyKb[]>(`${environment.apiUrl}/api/statistics/monthly-kbs?year=${year}`,
            { headers: this._sharedHeaders })
            .pipe(map((response: MonthlyKb[]) => {
                return response;
            }), catchError(this.handleError));
    }

    getMonthlyNewMembers(year: number) {
        return this.http.get<MonthlyMember[]>(`${environment.apiUrl}/api/statistics/monthly-registers?year=${year}`,
            { headers: this._sharedHeaders })
            .pipe(map((response: MonthlyMember[]) => {
                return response;
            }), catchError(this.handleError));
    }
}
