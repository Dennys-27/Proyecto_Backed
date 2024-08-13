﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;


namespace SistemaVenta.BLL.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _categoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        public async Task<CategoriaDTO> Crear(CategoriaDTO modelo)
        {
            try { 
            var categoriaCreada = await _categoriaRepositorio.Crear(_mapper.Map<Categoria>(modelo));
                if (categoriaCreada.IdCategoria == 0)
                {
                    throw new TaskCanceledException("No se pudo crear");
                }
                return _mapper.Map<CategoriaDTO>(categoriaCreada);
            }
            catch {
                throw;
            }
        }

        public async Task<bool> Editar(CategoriaDTO modelo)
        {
            try {
                var categoriaModelo = _mapper.Map<Categoria>(modelo);
                var categoriaEncontrada = await _categoriaRepositorio.Obtener(u => 
                u.IdCategoria == categoriaModelo.IdCategoria);

                if (categoriaEncontrada == null)
                     throw new TaskCanceledException("La categoria no existe");

                categoriaEncontrada.Nombre = categoriaModelo.Nombre;
                categoriaEncontrada.EsActivo = categoriaModelo.EsActivo;
              bool respuesta = await _categoriaRepositorio.Editar(categoriaEncontrada);

              if (!respuesta)
                 throw new TaskCanceledException("No se pudo editar");

                return respuesta;

            }
            catch {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var categoriaEncontrada  = await _categoriaRepositorio.Obtener(c => c.IdCategoria == id);

                if (categoriaEncontrada == null)
                    throw new TaskCanceledException("El categoria no existe");
                bool respuesta = await _categoriaRepositorio.Eliminar(categoriaEncontrada);

                if (respuesta == false)
                    throw new TaskCanceledException("No se pudo Eliminar");
                return respuesta;

            }
            catch {
                throw;
            }
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try {

                var listaCategorias = await _categoriaRepositorio.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategorias.ToList());
            
            } catch {
                throw;
            }
        }
    }
}