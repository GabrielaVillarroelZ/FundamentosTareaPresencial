using System;
using System.Collections.Generic;
using System.Linq;

public class Carta
{
    public string Palo { get; set; }
    public string Valor { get; set; }
    public int ValorNumerico { get; set; }

    public Carta(string palo, string valor, int valorNumerico)
    {
        Palo = palo;
        Valor = valor;
        ValorNumerico = valorNumerico;
    }

    public override string ToString() => $"{Valor} de {Palo}";
}

public class Program
{
    static Random random = new Random();
    static List<Carta> baraja = new List<Carta>();
    static Dictionary<string, int> cartasValor = new Dictionary<string, int>();

    public static void Main(string[] args)
    {
        while (true) // Bucle principal para permitir jugar varias rondas
        {
            Console.Clear();
            Console.WriteLine("¡Bienvenido al Blackjack!");
            Juego();

            Console.WriteLine("\n¿Quieres jugar otra ronda? (s/n): ");
            string respuesta = Console.ReadLine().ToLower();
            if (respuesta != "s")
            {
                Console.WriteLine("Gracias por jugar. ¡Hasta la próxima!");
                break; // Sale del bucle si el jugador no quiere jugar más
            }
        }
    }

    public static void Juego()
    {
        // Inicializar la baraja
        InicioBaraja();
        List<Carta> manoJugador = new List<Carta>();
        List<Carta> manoBanca = new List<Carta>();
        int puntuacionJugador = 0, puntuacionBanca = 0;

        // Repartir dos cartas al jugador y a la banca
        manoJugador.Add(CogerCarta());
        manoJugador.Add(CogerCarta());
        manoBanca.Add(CogerCarta());
        manoBanca.Add(CogerCarta());

        // Mostrar la mano inicial del jugador y la banca
        Console.WriteLine("\nMano del jugador:");
        foreach (var carta in manoJugador)
        {
            Console.WriteLine(carta);
        }

        MostrarManoBanca(manoBanca, false); // Solo muestra una carta de la banca

        // Turno del jugador
        while (true)
        {
            puntuacionJugador = CalcularPuntuacion(manoJugador);
            Console.WriteLine($"Puntuación actual del jugador: {puntuacionJugador}");

            if (puntuacionJugador > 21)
            {
                Console.WriteLine("¡Te has pasado de 21! Pierdes.");
                return;
            }

            Console.Write("¿Quieres otra carta? (s/n): ");
            if (Console.ReadLine().ToLower() != "s")
                break; // El jugador se planta

            manoJugador.Add(CogerCarta());
        }

        // Turno de la banca
        Console.WriteLine("\nTurno de la banca...");
        while (CalcularPuntuacion(manoBanca) < 17)
        {
            manoBanca.Add(CogerCarta());
            MostrarManoBanca(manoBanca, true);
        }

        // Determinar el ganador
        puntuacionBanca = CalcularPuntuacion(manoBanca);
        Console.WriteLine($"Puntuación final de la banca: {puntuacionBanca}");

        if (puntuacionBanca > 21 || puntuacionJugador > puntuacionBanca)
        {
            Console.WriteLine("¡Has ganado!");
        }
        else if (puntuacionBanca == puntuacionJugador)
        {
            Console.WriteLine("¡Es un empate!");
        }
        else
        {
            Console.WriteLine("La banca gana.");
        }
    }

    public static void InicioBaraja()
    {
        string[] palos = { "Corazones", "Tréboles", "Picas", "Diamantes" };
        string[] figuras = { "J", "Q", "K", "A" }; // Agregamos el As (A)

        baraja.Clear();  // Limpiar la baraja antes de cada ronda

        foreach (string palo in palos)
        {
            for (int i = 2; i <= 10; i++) // Del 2 al 10
            {
                baraja.Add(new Carta(palo, i.ToString(), i));
                cartasValor[i.ToString() + " de " + palo] = i;
            }

            foreach (string figura in figuras)
            {
                int valor = (figura == "A") ? 11 : 10; // As tiene valor 11, las figuras 10
                baraja.Add(new Carta(palo, figura, valor));
                cartasValor[figura + " de " + palo] = valor;
            }
        }
    }

    static Carta CogerCarta()
    {
        if (baraja.Count == 0)
            throw new InvalidOperationException("No quedan cartas en la baraja");

        int indice = random.Next(baraja.Count);
        Carta carta = baraja[indice];
        baraja.RemoveAt(indice);
        return carta;
    }

    static int CalcularPuntuacion(List<Carta> mano)
    {
        int puntuacion = mano.Sum(carta => carta.ValorNumerico);

        // Lógica para los ases: si hay ases, podemos cambiarlos de 1 a 11
        int cantidadAses = mano.Count(carta => carta.Valor == "A");
        for (int i = 0; i < cantidadAses; i++)
        {
            if (puntuacion + 10 <= 21)
            {
                puntuacion += 10;
            }
        }

        return puntuacion;
    }

    public static void MostrarManoBanca(List<Carta> mano, bool mostrarOculta = false)
    {
        Console.WriteLine("Mano de la banca:");
        if (mostrarOculta)
        {
            foreach (var carta in mano)
            {
                Console.WriteLine(carta.ToString());
            }
            Console.WriteLine($"Puntuación: {CalcularPuntuacion(mano)}");
        }
        else
        {
            // Mostrar solo la primera carta de la banca
            Console.WriteLine(mano[0].ToString());
            Console.WriteLine("Puntuación oculta");
        }
    }
}
