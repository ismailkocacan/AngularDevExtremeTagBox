import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

export class FilterModel{
  public productIds : Array<number>;
  constructor(){
    this.productIds = new Array<number>();
  }
}

@Injectable({
  providedIn: 'root'
})
export class DataService {

 constructor(private httpClient: HttpClient) { 

 }

convertToHttpParams(loadOptions:any) : HttpParams{    
  function isNotEmpty(value: any): boolean {
    return value !== undefined && value !== null && value !== "";
  }
  let params: HttpParams = new HttpParams();
  [
      "skip", 
      "take", 
      "requireTotalCount", 
      "requireGroupCount", 
      "sort", 
      "filter", 
      "totalSummary", 
      "group", 
      "groupSummary",
      "userData"
  ].forEach(function(i) {
      if (i in loadOptions && isNotEmpty(loadOptions[i])) 
        params = params.set(i, JSON.stringify(loadOptions[i]));
  }); 
  return params; 
}

getData(loadOptions: any){
  return this.httpClient.get('https://localhost:44334/api/product', { 
    params: this.convertToHttpParams(loadOptions) 
   });
}



}
