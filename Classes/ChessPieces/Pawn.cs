﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    /// <summary>
    /// Class representing a pawn on Chess board.
    /// </summary>
    public class Pawn : ChessPiece
    {
        public List<Coordinates> PossibleMoves { get; set; }

        /// <summary>
        /// Indicates whether this is the first move of the pawn.
        /// </summary>
        private bool _isThisTheFirstMove { get; set; } = true;

        /// <summary>
        /// Constructor of a Pawn class.
        /// </summary>
        /// <param name="startCoordinates">Coordinates where the pawn is placed at the beginning.</param>
        /// <param name="color">Color of the pwan.</param>
        public Pawn(Coordinates startCoordinates, string color)
        {
            CurrentCoordinates = startCoordinates;
            Color = color;
            Name = $"{color.Remove(1)}p";
        }

        /// <summary>
        /// Changes Pawns coordinates.
        /// </summary>
        /// <param name="newX">New X coordinate of the Pawn.</param>
        /// <param name="newY">New Y coordinate of the Pawn.</param>
        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }

        /// <summary>
        /// Checks whether move is legal.
        /// </summary>
        /// <param name="newX">X coordinate to move the pawn to.</param>
        /// <param name="newY">Y coordinate to move the pawn to.</param>
        /// <param name="Grid">Grid with all pieces on it.</param>
        /// <returns>True if the move is legal, false if illegal</returns>
        public bool IsMovePossible(int newX, int newY, string[,] Grid)
        {
            List<Coordinates> possibleCoordinates = new();

            if (Color == "white")
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X - 1, CurrentCoordinates.Y + 1));
                possibleCoordinates.Add(new(CurrentCoordinates.X - 1, CurrentCoordinates.Y - 1));
                PossibleMoves = possibleCoordinates;

                if (_isThisTheFirstMove 
                    && newX == CurrentCoordinates.X - 2 && newY == CurrentCoordinates.Y
                    && Grid[newX,newY] == " ")
                {
                    _isThisTheFirstMove = false;
                    return true;
                }
                if (newX == CurrentCoordinates.X - 1 && newY == CurrentCoordinates.Y
                    && Grid[newX, newY] == " ")
                {
                    _isThisTheFirstMove = false;
                    return true;
                }
                if (newX == CurrentCoordinates.X - 1 && newY == CurrentCoordinates.Y + 1
                    && Grid[newX, newY] != " ")
                {
                    _isThisTheFirstMove = false;
                    return true;
                }
                if (newX == CurrentCoordinates.X - 1 && newY == CurrentCoordinates.Y - 1
                    && Grid[newX, newY] != " ")
                {
                    _isThisTheFirstMove = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X + 1, CurrentCoordinates.Y + 1));
                possibleCoordinates.Add(new(CurrentCoordinates.X + 1, CurrentCoordinates.Y - 1));
                PossibleMoves = possibleCoordinates;

                if (_isThisTheFirstMove 
                    && newX == CurrentCoordinates.X + 2 && newY == CurrentCoordinates.Y
                    && Grid[newX, newY] == " ")
                {
                    _isThisTheFirstMove = false;
                    return true;
                }
                if (newX == CurrentCoordinates.X + 1 && newY == CurrentCoordinates.Y
                    && Grid[newX, newY] == " ")
                {
                    _isThisTheFirstMove = false;
                    return true;
                }
                if (newX == CurrentCoordinates.X + 1 && newY == CurrentCoordinates.Y + 1
                    && Grid[newX, newY] != " ")
                {
                    _isThisTheFirstMove = false;
                    return true;
                }
                if (newX == CurrentCoordinates.X + 1 && newY == CurrentCoordinates.Y - 1
                    && Grid[newX, newY] != " ")
                {
                    _isThisTheFirstMove = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
