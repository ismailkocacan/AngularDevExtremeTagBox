import { Component, ViewChild } from '@angular/core';
import { DxTagBoxComponent } from 'devextreme-angular';
import CustomStore from 'devextreme/data/custom_store';
import DataSource from 'devextreme/data/data_source';
import { DataService, FilterModel } from 'src/app/shared/services/data.service';

@Component({
  templateUrl: 'home.component.html',
  styleUrls: [ './home.component.scss' ]
})

export class HomeComponent {

  dsDatasource: DataSource;
  filter: FilterModel = new FilterModel();
  
  constructor(private dataService:DataService) {

    this.dsDatasource = new DataSource({ 
      store: new CustomStore({ 
        key: "id",
        loadMode: "raw",
          load: (loadOptions) => {
            console.log("load event");
            return dataService.getData(loadOptions)
            .toPromise()      
            .then((response: any) => {     
                 return response.data;           
              });
          }
        })
    });
    
    setInterval( ()=>{ 
      console.log("filter",this.filter);
    }, 2000);
  }
}
