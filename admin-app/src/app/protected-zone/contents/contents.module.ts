import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { BlockUIModule } from 'primeng/blockui';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { KeyFilterModule } from 'primeng/keyfilter';
import { TreeTableModule } from 'primeng/treetable';
import { DropdownModule } from 'primeng/dropdown';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ChipsModule } from 'primeng/chips';
import { FileUploadModule } from 'primeng/fileupload';
import { EditorModule } from 'primeng/editor';

import { SharedDirectivesModule } from '../../shared/directives/shared-directives.module';
import { NotificationService } from '../../shared/services';
import { ValidationMessageModule } from '../../shared/modules/validation-message/validation-message.module';

import { CategoriesComponent } from './categories/categories.component';
import { KnowledgeBasesComponent } from './knowledge-bases/knowledge-bases.component';
import { ContentsRoutingModule } from './contents-routing.module';
import { KnowledgeBasesDetailComponent } from './knowledge-bases/knowledge-bases-detail/knowledge-bases-detail.component';
import { CategoriesDetailComponent } from './categories/categories-detail/categories-detail.component';
import { CommentsDetailComponent } from './knowledge-bases/comments/comments-detail/comments-detail.component';
import { CommentsComponent } from './knowledge-bases/comments/comments.component';
import { ReportsComponent } from './knowledge-bases/reports/reports.component';
import { ReportsDetailComponent } from './knowledge-bases/reports/reports-detail/reports-detail.component';

@NgModule({
  declarations: [
    CategoriesComponent,
    KnowledgeBasesComponent,
    KnowledgeBasesDetailComponent,
    CategoriesDetailComponent,
    CommentsComponent,
    CommentsDetailComponent,
    ReportsComponent,
    ReportsDetailComponent
  ],
  imports: [
    CommonModule,
    ContentsRoutingModule,
    PanelModule,
    ButtonModule,
    TableModule,
    PaginatorModule,
    BlockUIModule,
    FormsModule,
    InputTextModule,
    ReactiveFormsModule,
    ProgressSpinnerModule,
    ValidationMessageModule,
    KeyFilterModule,
    CalendarModule,
    CheckboxModule,
    TreeTableModule,
    DropdownModule,
    InputTextareaModule,
    ChipsModule,
    FileUploadModule,
    EditorModule,
    SharedDirectivesModule,
    ModalModule.forRoot()
  ],
  providers: [
    NotificationService,
    BsModalService,
    DatePipe
  ]
})
export class ContentsModule { }
