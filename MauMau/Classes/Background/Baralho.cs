﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MauMau.Classes.Exceptions;
using MauMau.Classes.Background.Cartas;
using MauMau.Classes.Background.Cartas.Composicao;
using MauMau.Classes.Background.Estruturas;
using System.IO;
namespace MauMau.Classes.Background
{
    class Baralho
    {
        /// <summary>
        /// Lista de todas as cartas do baralho
        /// </summary>
        private Lista<Carta> cartas = new Lista<Carta>();
        private Random RAM = new Random();

        public Baralho()
        {
            LoadCards();
        }
        /// <summary>
        /// Carrega todas as cartas do baralho
        /// </summary>
        private void LoadCards()
        {
            try
            {
                foreach (Carta card in GetCardsListOfEspecificColor("amarelo")) cartas.Add(card);
                foreach (Carta card in GetCardsListOfEspecificColor("azul")) cartas.Add(card);
                foreach (Carta card in GetCardsListOfEspecificColor("verde")) cartas.Add(card);
                foreach (Carta card in GetCardsListOfEspecificColor("vermelho")) cartas.Add(card);
                foreach (Carta card in GetCardsListOfEspecificColor("especial")) cartas.Add(card);
            }
            catch(IOException  ee)
            {
                new ImageLoadException(ee.Message);
            }
            catch(NullReferenceException ee)
            {

            }
        }
        /// <summary>
        /// Retorna uma lista de cartas de acordo com a cor especificada
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Lista<Carta> GetCardsListOfEspecificColor(string color)
        {
            string corcarta = "";
            Lista<Carta> aux = new Lista<Carta>();
            switch (color)
            {
                case "amarelo":
                    corcarta = "yellow";
                    break;
                case "azul":
                    corcarta = "blue";
                    break;
                case "verde":
                    corcarta = "green";
                    break;
                case "vermelho":
                    corcarta = "red";
                    break;
                case "especial":
                    for (int i = 1; i <= 4; i++)//Quatro cartas coringa
                    {
                        aux.Add(CreateCuringaComEfeito());
                        aux.Add(CreateCuringaSemEfeito());
                    }
                    return aux;
                default:
                    return null;
            }

            for (int i = 0; i != 9; i++) aux.Add(CreateCard(color, i.ToString(), corcarta));
            for (int i = 1; i <= 2; i++) //Duas cartas especiais de cada um desses tipos
            {
                aux.Add(CreateCardSpecial(color, corcarta, "Bloq"));
                aux.Add(CreateCardSpecial(color, corcarta, "Compra"));
                aux.Add(CreateCardSpecial(color, corcarta, "Inverte"));
            }
            return aux;
        }

        /// <summary>
        /// Referido as cartas normais
        /// </summary>
        /// <param name="foldercolor"></param>
        /// <param name="cardNumber"></param>
        /// <param name="cardcolor"></param>
        private Carta CreateCard(string foldercolor, string cardNumber, string cardcolor)
        {
            ImageBrush brush;

            try { brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MauMau;component/Images/Cartas/" + foldercolor + "/" + cardNumber + cardcolor + ".jpg", UriKind.Absolute))); }
            catch { brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MauMau;component/Images/Cartas/" + foldercolor + "/" + cardNumber + cardcolor + ".png", UriKind.Absolute))); }

            return new Normal(PaletaCor.GetCor(foldercolor), int.Parse(cardNumber), brush);
        }

        /// <summary>
        /// Cria as cartas especiais de uma cor
        /// </summary>
        /// <param name="foldercolor"></param>
        /// <param name="cardcolor"></param>
        private Carta CreateCardSpecial(string foldercolor, string cardcolor, string cardname)
        {
            ImageBrush brush;

            try { brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MauMau;component/Images/Cartas/" + foldercolor + "/" + cardcolor + cardname + ".jpg", UriKind.Absolute))); }
            catch { brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MauMau;component/Images/Cartas/" + foldercolor + "/" + cardcolor + cardname + ".png", UriKind.Absolute))); }

            Efeito efeitoaux;
            if (cardname.ToUpper() == "BLOQ") efeitoaux = Efeito.Bloquear;
            else if (cardname.ToUpper() == "COMPRA") efeitoaux = Efeito.Comprar2;
            else efeitoaux = Efeito.Inverter;

            return new Especial(efeitoaux, brush, PaletaCor.GetCor(foldercolor));
        }

        private Carta CreateCuringaSemEfeito()
        {
            ImageBrush brush;

            try { brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MauMau;component/Images/Cartas/especial/coringa.jpg", UriKind.Absolute))); }
            catch { brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MauMau;component/Images/Cartas/especial/coringa.png", UriKind.Absolute))); }
            return new Coringa(brush, Efeito.MudarCor);
        }

        private Carta CreateCuringaComEfeito()
        {
            ImageBrush brush;

            try { brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MauMau;component/Images/Cartas/especial/coringaCompra.jpg", UriKind.Absolute))); }
            catch { brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MauMau;component/Images/Cartas/especial/coringaCompra.png", UriKind.Absolute))); }
            return new Coringa(brush, Efeito.MudarCorEComprar4);
        }

        public Lista<Carta> GetCards()
        {
            return this.cartas;
        }
        /// <summary>
        /// Reorganiza o baralho em uma nova ordem
        ///  int newplace = RAM.Next(0, cartas.Count - 1);
        ///  
        ///Carta aux = cartas[i];
        ///cartas[i] = cartas[newplace];
        ///cartas[newplace] = aux;
        ///
        /// </summary>
        public void Embaralhar()
        {
            if (cartas.Count > 1)
            {
                for (int i = 0; i < cartas.Count; i++)
                {
                    int newplace = RAM.Next(0, cartas.Count - 1);
                    Carta aux = cartas[i];
                    cartas[i] = cartas[newplace];
                    cartas[newplace] = aux;
                }        
            }
        }
    }
}
