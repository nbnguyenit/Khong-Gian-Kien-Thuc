import { Component, OnInit } from '@angular/core';
import { StatisticsService } from '../../../shared/services';
import { BaseComponent } from '../../../protected-zone/base/base.component';

@Component({
  selector: 'app-monthly-comments',
  templateUrl: './monthly-comments.component.html',
  styleUrls: ['./monthly-comments.component.css']
})
export class MonthlyCommentsComponent extends BaseComponent implements OnInit {
  // Default
  public blockedPanel = false;
  // Customer Receivable
  public items: any[];
  public year: number = new Date().getFullYear();
  public totalItems = 0;
  constructor(
    private statisticService: StatisticsService) {
      super('STATISTIC_MONTHLY_COMMENT');
  }
  ngOnInit() {
    super.ngOnInit();
    this.loadData();
  }
  loadData() {
    this.blockedPanel = true;
    this.statisticService.getMonthlyNewComments(this.year)
      .subscribe((response: any) => {
        this.totalItems = 0;
        this.items = response;
        response.forEach(element => {
          this.totalItems += element.NumberOfUsers;
        });
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      }, error => {
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      });
  }

}
