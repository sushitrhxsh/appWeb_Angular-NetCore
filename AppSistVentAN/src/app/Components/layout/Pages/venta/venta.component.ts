import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';

import { Venta } from 'src/app/Interfaces/venta';
import { Producto } from 'src/app/Interfaces/producto';
import { DetalleVenta } from 'src/app/Interfaces/detalle-venta';
import { ProductoService } from 'src/app/Services/producto.service';
import { VentaService } from 'src/app/Services/venta.service';
import { UtilidadService } from 'src/app/Reutilizable/utilidad.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-venta',
  templateUrl: './venta.component.html',
  styleUrls: ['./venta.component.css']
})
export class VentaComponent implements OnInit {

  listaProductos:         Producto[] = [];
  listaProductosFiltro:   Producto[] = [];
  listaProductosParaVenta:DetalleVenta[] = [];
  bloquearBotonRegistrar: boolean = false;
  productoSeleccionado!:  Producto;
  tipodePagoPorDefecto:   string = "Efectivo";
  totalPagar:             number = 0;

  formularioProductoVenta:FormGroup;
  columnasTabla:string[] = ["producto","cantidad","precio","total","accion"];
  datosDetalleVenta = new MatTableDataSource(this.listaProductosParaVenta);

  retornarProductosPorFiltro(busqueda:any):Producto[]{
    const valorBuscado = typeof busqueda === "string" ? busqueda.toLocaleLowerCase():busqueda.nombre.toLocaleLowerCase();

    return this.listaProductos.filter(item => item.nombre.toLocaleLowerCase().includes(valorBuscado));
  }

  constructor(
    private fb:FormBuilder,
    private _productoService:ProductoService,
    private _ventaService:VentaService,
    private _utilidadService:UtilidadService
  ){
    this.formularioProductoVenta = fb.group({
      producto: ["",Validators.required],
      cantidad: ["",Validators.required],
    });

    this._productoService.lista().subscribe({
      next:(data) => {
        if(data.status){
          const lista = data.value as Producto[];
          this.listaProductos = lista.filter(p => p.esActivo == 1 && p.stock > 0);
        }
      },
      error:(e) => {}
    });

    this.formularioProductoVenta.get("producto")?.valueChanges.subscribe(value => {
      this.listaProductosFiltro = this.retornarProductosPorFiltro(value);
    });

   }

  ngOnInit(): void {
  }

  mostrarProducto(producto:Producto):string{
    return producto.nombre;
  }

  productoParaVenta(event:any){
    this.productoSeleccionado = event.option.value;
  }

  agregarProductoParaVenta(){
    const cantidad:number = this.formularioProductoVenta.value.cantidad;
    const precio:number   = parseFloat(this.productoSeleccionado.precio);
    const total:number    = cantidad * precio;
    this.totalPagar       = this.totalPagar + total;

    this.listaProductosParaVenta.push({ // 20:42 c12
      idProducto:           this.productoSeleccionado.idProducto,
      descripcionProducto:  this.productoSeleccionado.nombre,
      cantidad:             cantidad,
      precioTexto:          String(precio.toFixed(2)),
      totalTexto:           String(total.toFixed(2))
    });

    this.datosDetalleVenta = new MatTableDataSource(this.listaProductosParaVenta);
    
    this.formularioProductoVenta.patchValue({
      producto:"",
      cantidad:""
    });
  }

  eliminarProducto(detalle:DetalleVenta){
    this.totalPagar = this.totalPagar - parseFloat(detalle.totalTexto);
    this.listaProductosParaVenta = this.listaProductosParaVenta.filter(p => p.idProducto != detalle.idProducto);

    this.datosDetalleVenta = new MatTableDataSource(this.listaProductosParaVenta);
  }

  registrarVenta(){
    if(this.listaProductosParaVenta.length > 0){
      this.bloquearBotonRegistrar = true;

      const request: Venta = {
        tipoPago:     this.tipodePagoPorDefecto,
        totalTexto:   String(this.totalPagar.toFixed(2)),
        detalleVenta: this.listaProductosParaVenta
      }

      this._ventaService.registrar(request).subscribe({
        next:(response) => {
          if(response.status){
            this.totalPagar = 0.00;
            this.listaProductosParaVenta = [];
            this.datosDetalleVenta = new MatTableDataSource(this.listaProductosParaVenta);

            Swal.fire({
              title:  "Venta Registrada",
              text:   `Numero de venta: ${response.value.numeroDocumento}`,
              icon:   "success",  
            });

          } else {
            this._utilidadService.mostrarAlerta("No se pudo registrar la venta", "Opps!");
          }
        },
        complete:() => {
          this.bloquearBotonRegistrar = false;
        },
        error:(e) => { }
      });

    }
  }

}