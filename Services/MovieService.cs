using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data;
using MoviesAPI.Data.Dtos;
using MoviesAPI.Models;

public class MovieService
{
    private readonly MovieContext _context;
    private readonly IMapper _mapper;

    public MovieService(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Movie CreateMovie(CreateMovieDto movieDto)
    {
        Movie movie = _mapper.Map<Movie>(movieDto);
        _context.Movies.Add(movie);
        _context.SaveChanges();
        return movie;
    }

    public IQueryable<Movie> GetMovies(int skip, int take)
    {
        return _context.Movies.Skip(skip).Take(take);
    }

    public Movie GetMovieById(int id)
    {
        return _context.Movies.FirstOrDefault(movie => movie.Id == id);
    }

    public bool UpdateMovie(int id, UpdateMovieDto movieDto)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return false;
        _mapper.Map(movieDto, movie);
        _context.SaveChanges();
        return true;
    }

    public Movie UpdatePartialMovie(int id, UpdateMovieDto moviePatch)
    {
        var movieToUpdate = _context.Movies.FirstOrDefault(p => p.Id == id);
        if (movieToUpdate != null)
        {
            movieToUpdate.Title = moviePatch.Title;
            movieToUpdate.Gender = moviePatch.Gender;
            movieToUpdate.Duration = moviePatch.Duration;

            return movieToUpdate;
        }

        return null;
    }

    public bool DeleteMovie(int id)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null) return false;
        _context.Remove(movie);
        _context.SaveChanges();
        return true;
    }
}
