using System;
using System.Collections.Generic;

[Serializable]
public class Root
{
    public string categoria;
    public List<CasoClinico> casos;
}

[Serializable]
public class CasoClinico
{
    public int id;
    public string titulo;

    public InformacionPaciente informacionPaciente;

    public List<string> sintomas;

    public PreguntaSimple patronMarcha;
    public PreguntaSimple cuadroPatologico;

    public PreguntaMultiple manifestacionesPropias;

    public Solucion solucion;
}

[Serializable]
public class InformacionPaciente
{
    public string nombre;
    public int edad;
    public string sexo;
    public List<string> incidentesPrevios;
}

[Serializable]
public class PreguntaSimple
{
    public string pregunta;
    public List<string> opciones;
    public int correcta;
}

[Serializable]
public class PreguntaMultiple
{
    public string pregunta;
    public List<string> opciones;
    public List<int> correctas;
}

[Serializable]
public class Solucion
{
    public string patronCorrecto;
    public string cuadroPatologicoCorrecto;
    public List<string> manifestacionesCorrectas;
}