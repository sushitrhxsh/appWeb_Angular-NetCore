import { Component, OnInit } from '@angular/core';

import { Chart,registerables } from 'chart.js';
import { DashBoardService } from 'src/app/Services/dash-board.service';
Chart.register(...registerables);

@Component({
  selector: 'app-dash-board',
  templateUrl: './dash-board.component.html',
  styleUrls: ['./dash-board.component.css']
})
export class DashBoardComponent implements OnInit {

  totalIngresos:  string = "0";
  totalVentas:    string = "0";
  totalProductos: string = "0";
  

  constructor(
    private _dashboardService:DashBoardService
  ){ }

  mostrarGrafico(labelGrafico:any[], dataGrafico:any[]){
    const chartBar = new Chart('chartBar', {
      type: "bar",
      data: {
        labels: labelGrafico,
        datasets: [{
          label:           "Numero de ventas",
          data:            dataGrafico,
          backgroundColor: ["rgba(211, 89, 222, 0.8)"],
          borderColor:     ["purple"],
          borderWidth:     1
        }]
      },
      options: {
        maintainAspectRatio: false,
        responsive:          true,
        scales: {
          y: { beginAtZero: true }
        }
      }
    });
  }

  ngOnInit(): void {
    this._dashboardService.resumen().subscribe({
      next:(data) => {
        if(data.status){
          this.totalIngresos  = data.value.totalIngresos;
          this.totalVentas    = data.value.totalVentas;
          this.totalProductos = data.value.totalProductos;
          
          const arrayData:any[] = data.value.ventasUltimaSemana;
          console.log(arrayData);

          const labelTemp = arrayData.map((v) => v.fecha);
          const dataTemp  = arrayData.map((v) => v.total);
          console.log(labelTemp,dataTemp);

          this.mostrarGrafico(labelTemp,dataTemp);
        }
      },
      error:(e) => { }
    });
  }

}