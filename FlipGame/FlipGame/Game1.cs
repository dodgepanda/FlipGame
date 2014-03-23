using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FlipGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int BOARD_WIDTH = 4;
        const int BOARD_HEIGHT = 4;
        const int PIECE_SPACING = 1;
        const float SOLVE_INTERVAL = 1f;
        Vector2 BOARD_LOCATION;
        GraphicsDeviceManager graphics;
        Viewport vp;
        SpriteBatch spriteBatch;
        Texture2D whiteCircleSprite;
        Texture2D solutionButtonSprite;
        Texture2D allWhiteBtnSprite;
        Texture2D allBlackBtnSprite;
        Vector2 allWhiteBtnLoc;
        Vector2 allBlackBtnLoc;
        Vector2 solutionButtonLocation;
        Texture2D newGameSprite;
        Vector2 newGameLocation;
        SpriteFont font1;
        FlipBoard fb;
        Vector2[,] pieceLocation;
        bool mouseDown = false;
        int numMoves = 0;
        Stack<FlipBoard> solution;
        bool showSolution = false;
        int solutionMoves = -1;
        bool noSolution = false;

        float totalTimeElapsed = 0;
        float lastSolutionTime = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.PreferredBackBufferWidth = 1000;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            BOARD_LOCATION = new Vector2(10, 10);
            vp = graphics.GraphicsDevice.Viewport;
            fb = new FlipBoard(BOARD_WIDTH, BOARD_HEIGHT);
            solution = new Stack<FlipBoard>();
            pieceLocation = new Vector2[(int)fb.getNumXY().X, (int)fb.getNumXY().Y];
            base.Initialize();
            fb.newGame();
            //Testing
            //fb.test();
            //List<FlipBoard> fbs = new List<FlipBoard>();
            //fbs.AddRange(fb.generateChildren());
            //foreach (FlipBoard ff in fbs)
            //{
            //    Console.WriteLine(ff.toString());
            //    Console.WriteLine("Is game over? " + ff.isGameOver());
            //}
            //fbs[0].flipPiece(2, 2);
            //fbs[0].flipPiece(3, 3);

            //FlipBoardSolver solver = new FlipBoardSolver(fb);
            //FlipBoard winner = solver.getWinner();
            //if (winner != null)
            //    Console.WriteLine(winner.toString());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            whiteCircleSprite = Content.Load<Texture2D>(@"Sprites\white");
            solutionButtonSprite = Content.Load<Texture2D>(@"Sprites\solutionButton");
            allWhiteBtnSprite = Content.Load<Texture2D>(@"Sprites\allWhite");
            allBlackBtnSprite = Content.Load<Texture2D>(@"Sprites\allBlack");
            newGameSprite = Content.Load<Texture2D>(@"Sprites\newGame");
            font1 = Content.Load<SpriteFont>(@"Fonts\font1");

            solutionButtonLocation = new Vector2(10 + fb.getNumXY().X * whiteCircleSprite.Width + 10, 50);
            allBlackBtnLoc = new Vector2(solutionButtonLocation.X + solutionButtonSprite.Width + 10, 50);
            allWhiteBtnLoc = new Vector2(allBlackBtnLoc.X + allBlackBtnSprite.Width + 10, 50);
            newGameLocation = new Vector2(10 + fb.getNumXY().X * whiteCircleSprite.Width + 10, solutionButtonLocation.Y+40);
            createPieces();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            totalTimeElapsed += elapsed;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            int mouseX = 0;
            int mouseY = 0;
            // TODO: Add your update logic here
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !mouseDown)
            {
                mouseDown = true;
                mouseX = Mouse.GetState().X;
                mouseY = Mouse.GetState().Y;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                mouseDown = false;
            Rectangle mouseRec = new Rectangle(mouseX, mouseY, 0, 0);
            if(!fb.isGameOver() && !noSolution)
            {
                Circle mouseCirc = new Circle(mouseX, mouseY, 0);
                for (int i = 0; i < fb.getNumXY().X; i++)
                {
                    for (int j = 0; j < fb.getNumXY().Y; j++)
                    {
                        Circle pieceCirc = new Circle((int)pieceLocation[i, j].X, (int)pieceLocation[i, j].Y, whiteCircleSprite.Width/2);
                        if(mouseCirc.intersects(pieceCirc))
                        {
                            fb.flipPiece(i, j);
                            numMoves++;
                        }
                    }
                }
                Rectangle solutionRec = new Rectangle((int)solutionButtonLocation.X, (int)solutionButtonLocation.Y, solutionButtonSprite.Width, solutionButtonSprite.Height);
                Rectangle blackSolutionRec = new Rectangle((int)allBlackBtnLoc.X, (int)allBlackBtnLoc.Y, allBlackBtnSprite.Width, allBlackBtnSprite.Height);
                Rectangle whiteSolutionRec = new Rectangle((int)allWhiteBtnLoc.X, (int)allWhiteBtnLoc.Y, allWhiteBtnSprite.Width, allWhiteBtnSprite.Height);
                int solveColor = 0;
                if ((mouseRec.Intersects(solutionRec) || mouseRec.Intersects(blackSolutionRec) || mouseRec.Intersects(whiteSolutionRec)) && !showSolution)
                {
                    if (mouseRec.Intersects(blackSolutionRec))
                        solveColor = -1;
                    if (mouseRec.Intersects(whiteSolutionRec))
                        solveColor = 1;
                    FlipBoardSolver fbs = new FlipBoardSolver(fb, solveColor);
                    solution = fbs.getSolution();
                    showSolution = true;
                }
                if (showSolution && lastSolutionTime + SOLVE_INTERVAL <= totalTimeElapsed)
                {
                    lastSolutionTime = totalTimeElapsed;
                    if (solution.Count() != 0)
                        fb = solution.Pop();
                    else
                        noSolution = true;
                    solutionMoves++;
                }
            }

            Rectangle newgameRec = new Rectangle((int)newGameLocation.X, (int)newGameLocation.Y, newGameSprite.Width, newGameSprite.Height);
            if (mouseRec.Intersects(newgameRec))
                newGame();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();  //Use spriteBatch to draw offscreen
            // TODO: Add your drawing code here
            for (int i = 0; i < fb.getNumXY().X; i++)
            {
                for (int j = 0; j < fb.getNumXY().Y; j++)
                {
                    spriteBatch.Draw(whiteCircleSprite, pieceLocation[i,j], fb.getPieceColor(i,j));
                }
            }
            spriteBatch.DrawString(font1, "Moves: " + numMoves, new Vector2(10 + fb.getNumXY().X * whiteCircleSprite.Width + 10, 10), Color.White);
            spriteBatch.Draw(solutionButtonSprite, solutionButtonLocation, Color.White);
            spriteBatch.Draw(allBlackBtnSprite, allBlackBtnLoc, Color.White);
            spriteBatch.Draw(allWhiteBtnSprite, allWhiteBtnLoc, Color.White);
            spriteBatch.Draw(newGameSprite, newGameLocation, Color.White);
            if (fb.isGameOver())
            {
                spriteBatch.DrawString(font1, "Win!", new Vector2(10, fb.getNumXY().Y * whiteCircleSprite.Height + 10), Color.White);
                if(showSolution)
                    spriteBatch.DrawString(font1, "Solution Finder Solved in " + solutionMoves + " moves." , new Vector2(10, fb.getNumXY().Y * whiteCircleSprite.Height + 30), Color.White);
            }
            if (noSolution)
                spriteBatch.DrawString(font1, "No Solution", new Vector2(10, fb.getNumXY().Y * whiteCircleSprite.Height + 50), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void createPieces()
        {
            for (int i = 0; i < fb.getNumXY().X; i++)
            {
                for (int j = 0; j < fb.getNumXY().Y; j++)
                {
                    pieceLocation[i, j] = new Vector2(BOARD_LOCATION.X + i * whiteCircleSprite.Width, BOARD_LOCATION.Y + j * whiteCircleSprite.Height);
                }
            }
        }

        private void newGame()
        {
            fb.newGame();
            numMoves = 0;
            showSolution = false;
            solutionMoves = -1;
            solution = new Stack<FlipBoard>();
            noSolution = false;
        }
    }
}
