import { Component, EventEmitter, OnInit } from '@angular/core';
import { CommandAssign } from '../../../../shared/models';
import { CommandsService } from '../../../../shared/services/commands.service';
import { FunctionsService } from '../../../../shared/services/functions.service';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-commands-assign',
  templateUrl: './commands-assign.component.html',
  styleUrls: ['./commands-assign.component.scss']
})
export class CommandsAssignComponent implements OnInit {
  public blockedPanel = false;
  public items: any[];
  public selectedItems: any[] = [];
  public dialogTitle: string;
  public functionId: string;
  public existingCommands: any[] = [];
  public addToAllFunctions = false;
  private chosenEvent: EventEmitter<any> = new EventEmitter();

  constructor(
    public bsModalRef: BsModalRef,
    private functionsService: FunctionsService,
    private commandsService: CommandsService) {
  }

  ngOnInit() {
    this.loadAllCommands();
  }

  loadAllCommands() {
    this.blockedPanel = true;
    this.commandsService.getAll()
      .subscribe((response: any) => {
        this.items = [];

        const existingCommands = this.existingCommands;
        const notExistingCommands = response.filter(function (item) {
          return existingCommands.indexOf(item.Id) === -1;
        });

        this.items = notExistingCommands;
        if (this.selectedItems.length === 0 && this.items.length > 0) {
          this.selectedItems.push(this.items[0]);
        }
        setTimeout(() => { this.blockedPanel = false; }, 1000);
      });
  }


  chooseCommands() {
    this.blockedPanel = true;
    const selectedItemIds = [];
    this.selectedItems.forEach(element => {
      selectedItemIds.push(element.id);
    });
    const entity = new CommandAssign();
    entity.addToAllFunctions = this.addToAllFunctions;
    entity.commandIds = selectedItemIds;

    this.functionsService.addCommandsToFunction(this.functionId, entity).subscribe(() => {
      this.chosenEvent.emit(this.selectedItems);
      setTimeout(() => { this.blockedPanel = false; }, 1000);
    });
  }
}
