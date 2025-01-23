using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using jumpE_basic;
using System;
using System.Collections.Generic;
using System.IO;

namespace JE_text
{
    public class program
    {
        public static void Main(string[] args)
        {
            using (var game = new HomePage())
            {
                game.Run();
            }
        }
    }
    public class HomePage : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        string inputPath = string.Empty;
        List<string> previousPaths = new List<string>();
        const string filePath = "lastTenPaths.txt";

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public HomePage()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            LoadPaths();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial"); // Only use the file name without the extension
        }


        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                AddPath(inputPath);
                inputPath = string.Empty;
            }
            else
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (previousKeyboardState.IsKeyUp(key))
                    {
                        if (key == Keys.Back && inputPath.Length > 0)
                        {
                            inputPath = inputPath.Substring(0, inputPath.Length - 1);
                        }
                        else if (key != Keys.Back && key != Keys.Enter)
                        {
                            inputPath += key.ToString();
                        }
                    }
                }
            }

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Enter Path:", new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(font, inputPath, new Vector2(100, 150), Color.White);

            int yOffset = 200;
            for (int i = 0; i < previousPaths.Count; i++)
            {
                spriteBatch.DrawString(font, previousPaths[i], new Vector2(100, yOffset), Color.White);
                yOffset += 30;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void AddPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                previousPaths.Insert(0, path);
                if (previousPaths.Count > 10)
                {
                    previousPaths.RemoveAt(10);
                }

                SavePaths();
            }
        }

        private void SavePaths()
        {
            File.WriteAllLines(filePath, previousPaths);
        }

        private void LoadPaths()
        {
            if (File.Exists(filePath))
            {
                previousPaths = new List<string>(File.ReadAllLines(filePath));
            }
        }
    }
}

