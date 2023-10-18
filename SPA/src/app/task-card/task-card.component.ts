import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskCategory, TaskStatus, Tasks } from '../models/tasks';
import { OnChanges } from '@angular/core';
import { formatDate } from '@angular/common';
import { TasksService } from '../services/tasks.service';
@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.css'],
})
export class TaskCardComponent implements OnChanges {
  @Input() task: Tasks;

  taskForm: FormGroup;
  TaskCategory = TaskCategory;
  TaskStatus = TaskStatus;
  taskCategories: string[];
  taskStatuses: string[];

  constructor(
    private formBuilder: FormBuilder,
    private taskService: TasksService
  ) {
    this.taskCategories = Object.keys(this.TaskCategory);
    this.taskStatuses = Object.keys(this.TaskStatus);
    this.taskForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', Validators.required],
      dueDate: ['', Validators.required],
      category: ['', Validators.required],
      status: ['', Validators.required],
    });
    this.taskForm.disable();
  }

  ngOnChanges(changes: any): void {
    this.updateForm(this.task);
  }

  updateForm(data: Tasks): void {
    //let x = this.task.dueDate.getUTCDate();

    if (!(this.task.dueDate instanceof Date)) {
      // Convert 'this.task.dueDate' to a Date object if it's not
      this.task.dueDate = new Date(this.task.dueDate);
    }

    let formattedDate = formatDate(this.task.dueDate, 'yyyy-MM-dd', 'en-US');

    this.taskForm.patchValue({
      name: this.task.name,
      description: this.task.description,
      category: this.taskCategories[this.task.category],
      status: this.taskStatuses[this.task.status],
      dueDate: formattedDate,
    });
  }

  edit() {
    this.taskForm.enable();
  }

  discard() {
    this.taskForm.reset();
    this.taskForm.disable();
  }

  onSubmit() {}

  Delete() {
    this.taskService.deleteTask(this.task.id).subscribe();
  }
}
