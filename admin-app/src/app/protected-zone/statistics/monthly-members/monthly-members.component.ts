import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../protected-zone/base/base.component';
import { StatisticsService } from '../../../shared/services';

@Component({
  selector: 'app-monthly-members',
  templateUrl: './monthly-members.component.html',
  styleUrls: ['./monthly-members.component.css']
})
export class MonthlyMembersComponent extends BaseComponent implements OnInit {

  // Default
  public blockedPanel = false;
  // Customer Receivable
  public items: any[];
  public year: number = new Date().getFullYear();
  public totalItems = 0;
  constructor(
    private statisticService: StatisticsService) {
     super('STATISTIC_MONTHLY_NEWMEMBER');
  }
  ngOnInit() {
    super.ngOnInit();
    this.loadData();
  }
  loadData() {
    this.blockedPanel = true;
    this.statisticService.getMonthlyNewMembers(this.year)
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
