import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../protected-zone/base/base.component';
import { StatisticsService } from '../../../shared/services';

@Component({
  selector: 'app-monthly-new-kbs',
  templateUrl: './monthly-kbs.component.html',
  styleUrls: ['./monthly-kbs.component.css']
})
export class MonthlyKbsComponent extends BaseComponent implements OnInit {
  // Default
  public blockedPanel = false;
  // Customer Receivable
  public items: any[];
  public year: number = new Date().getFullYear();
  public totalItems = 0;
  constructor(
    private statisticService: StatisticsService) {
      super('STATISTIC_MONTHLY_NEWKB');
  }
  ngOnInit() {
    super.ngOnInit();
    this.loadData();
  }
  loadData() {
    this.blockedPanel = true;
    this.statisticService.getMonthlyNewKbs(this.year)
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
