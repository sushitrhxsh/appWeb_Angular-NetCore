import { Component, OnInit, Inject } from '@angular/core';

import { MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Venta } from 'src/app/Interfaces/venta';
import { DetalleVenta } from 'src/app/Interfaces/detalle-venta';


@Component({
  selector: 'app-modal-detalle-venta',
  templateUrl: './modal-detalle-venta.component.html',
  styleUrls: ['./modal-detalle-venta.component.css']
})
export class ModalDetalleVentaComponent implements OnInit {

  fechaRegistro:string = "";
  numeroDocumento:string = "";
  tipoPago:string = "";
  total:string = "";
  detalleVenta:DetalleVenta[] = [];
  columnasTabla:string[] = ['producto','cantidad','precio','total'];


  constructor(
      @Inject(MAT_DIALOG_DATA) public datosVenta: Venta
  ){ 
      this.fechaRegistro    = datosVenta.fechaRegistro!;
      this.numeroDocumento  = datosVenta.numeroDocumento!;
      this.tipoPago         = datosVenta.tipoPago;
      this.total            = datosVenta.totalTexto;
      this.detalleVenta     = datosVenta.detalleVenta;
   }

  ngOnInit(): void {
  }

}
