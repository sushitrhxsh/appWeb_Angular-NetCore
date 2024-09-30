import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import * as moment from 'moment';
import * as XLSX from 'xlsx';

import { Reporte } from 'src/app/Interfaces/reporte';
import { VentaService } from 'src/app/Services/venta.service';
import { UtilidadService } from 'src/app/Reutilizable/utilidad.service';


export const MY_DATA_FORMATS = {
  parse:{
    dateInput:"DD/MM/YYYY"
  },
  display:{
    dateInput:"DD/MM/YYYY",
    monthYearLabel:"MMMMM YYYY"
  }
}

@Component({
  selector: 'app-reporte',
  templateUrl: './reporte.component.html',
  styleUrls: ['./reporte.component.css'],
  providers:[{
    provide:MAT_DATE_FORMATS, useValue:MY_DATA_FORMATS
  }]
})
export class ReporteComponent implements OnInit {

  formularioFiltro:FormGroup;
  listaVentasReporte:Reporte[] = [];
  columnasTabla:string[] = ["fechaRegistro","numeroVenta","tipoPago","total","producto","cantidad","precio","totalProducto"];
  datosVentaReporte = new MatTableDataSource(this.listaVentasReporte);
  @ViewChild(MatPaginator) paginacionTabla!:MatPaginator;


  constructor(
    private fb:FormBuilder,
    private _ventaService:VentaService,
    private _utilidadService:UtilidadService
  ){
    this.formularioFiltro = this.fb.group({
      fechaInicio:  ["", Validators.required],
      fechaFin:     ["", Validators.required],
    });
   }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.datosVentaReporte.paginator = this.paginacionTabla;
  }

  buscarVentas(){
    const fecha_inicio  = moment(this.formularioFiltro.value.fechaInicio).format("DD/MM/YYYY");
    const fecha_fin     = moment(this.formularioFiltro.value.fechaFin).format("DD/MM/YYYY");

    if(fecha_inicio === "invalid date" || fecha_fin === "invalid date"){
      this._utilidadService.mostrarAlerta("Debe ingresar ambas fechas","Opps");
      return;
    }

    this._ventaService.reporte(fecha_inicio,fecha_fin).subscribe({
      next:(data) => {
        if(data.status){
          this.listaVentasReporte = data.value;
          this.datosVentaReporte.data = data.value;
        } else {
          this.listaVentasReporte = [];
          this.datosVentaReporte.data = [];
          this._utilidadService.mostrarAlerta("No se encontraron datos.","Opps")
        }
      },
      error:(e) => { }
    });

  }

  exportarExcel(){
    const wb = XLSX.utils.book_new(); // book
    const ws = XLSX.utils.json_to_sheet(this.listaVentasReporte); // sheet 

    XLSX.utils.book_append_sheet(wb,ws,"Reporte");
    XLSX.writeFile(wb,"ReporteVentas.xlsx");
  }

}
