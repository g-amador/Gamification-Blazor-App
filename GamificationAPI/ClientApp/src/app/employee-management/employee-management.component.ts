import { Component, ViewChild, Inject } from '@angular/core';
import { MatDialog, MatTable } from '@angular/material';
import { DialogBoxComponent } from './dialog-box/dialog-box.component';
import { HttpClient } from '@angular/common/http';

export interface UsersData {
  name: string;
  id: number;
}

@Component({
  selector: 'app-employee-management-component',
  templateUrl: './employee-management.component.html',
  styleUrls: ['./employee-management.component.css']
})
export class EmployeeManagementComponent {
  displayedColumns: string[] = ['name', 'age', 'designation', 'mobileNumber', 'action'];
  dataSource: any;  
  public employee: Employee;
  @ViewChild(MatTable,{static:true}) table: MatTable<any>;

  constructor(public dialog: MatDialog, public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {
    http.get<Employee[]>(baseUrl + 'api/employees').subscribe(result => {
       this.dataSource = result;
    }, error => console.error(error));
  }

  openDialog(action,obj) {
    obj.action = action;
    const dialogRef = this.dialog.open(DialogBoxComponent, {
      width: '300px',
      data:obj
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result.event == 'Add'){
        this.addRowData(result.data);
      }else if(result.event == 'Update'){
        this.updateRowData(result.data);
      }else if(result.event == 'Delete'){
        this.deleteRowData(result.data);
      }
    });
  }

  addRowData(newRow){  
    var result = this.dataSource.push({      
      name:newRow.name,
      age:newRow.age,
      designation:newRow.designation,
      mobileNumber:newRow.mobileNumber
    });

    this.employee = {       
      id:result.id,
      name:newRow.name,
       age: parseInt(newRow.age),
       designation:newRow.designation,
       mobileNumber:parseInt(newRow.mobileNumber)
    };
    const headers = { 'content-type': 'application/json'}  
    const body=JSON.stringify(this.employee);
    this.http.post<Employee>(this.baseUrl + 'api/Employees', body , {'headers':headers}).subscribe(data => {        
    })

    this.table.renderRows();
    
  }
  updateRowData(newRow){
    this.dataSource = this.dataSource.filter((value,key)=>{
      if(value.id == newRow.id){
          value.name = newRow.name;
          value.age=newRow.age;
          value.designation=newRow.designation;
          value.mobileNumber=newRow.mobileNumber;
      }

      this.employee = {    
        id: newRow.id,
        name:newRow.name,
        age: parseInt(newRow.age),
        designation:newRow.designation,
        mobileNumber:parseInt(newRow.mobileNumber)
      };
      const headers = { 'content-type': 'application/json'}  
      const body=JSON.stringify(this.employee);
      this.http.put<Employee[]>(this.baseUrl + 'api/Employees/' + newRow.id, body , {'headers':headers}).subscribe(data => {         
      })
      return true;
    });
  }
  deleteRowData(newRow){
    this.dataSource = this.dataSource.filter((value,key)=>{
       this.http.delete<any>(this.baseUrl + 'api/Employees/' + newRow.id).subscribe(data => {         
      })
      return value.id != newRow.id;
    });
  }
}

interface Employee {
  id: string;
  name: number;
  age: number;
  designation: string;
  mobileNumber: number;
}
