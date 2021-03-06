﻿namespace TransportDialogPlugin
{
    using GameFreeText;
    using GameGlobal;
    using GameObjects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using PluginInterface;
    using System;
    using System.Collections.Generic;

    public class TransportDialog
    {
        internal Point BackgroundSize;
        internal Texture2D BackgroundTexture;
        internal int Days;
        private Architecture destinationArchitecture;
        internal Texture2D DestinationButtonDisplayTexture;
        internal Rectangle DestinationButtonPosition;
        internal Texture2D DestinationButtonSelectedTexture;
        internal Texture2D DestinationButtonTexture;
        internal FreeText DestinationCommentText;
        internal FreeText DestinationText;
        internal Point DisplayOffset;
        internal IGameFrame GameFramePlugin;
        internal Texture2D InputNumberButtonDisplayTexture;
        internal Rectangle InputNumberButtonPosition;
        internal Texture2D InputNumberButtonSelectedTexture;
        internal Texture2D InputNumberButtonTexture;
        internal FreeText InputNumberText;
        private bool isShowing;
        internal TransportKind Kind;
        internal List<LabelText> LabelTexts = new List<LabelText>();
        private int number;
        internal INumberInputer NumberInputerPlugin;
        internal IGameRecord GameRecordPlugin;
        private Screen screen;
        internal Architecture SourceArchitecture;
        internal Texture2D StartButtonDisabledTexture;
        internal Texture2D StartButtonDisplayTexture;
        internal bool StartButtonEnabled;
        internal Rectangle StartButtonPosition;
        internal Texture2D StartButtonSelectedTexture;
        internal Texture2D StartButtonTexture;
        internal float SurplusRate;
        internal ITabList TabListPlugin;
        internal FreeText TitleText;

        internal void Draw(SpriteBatch spriteBatch)
        {
            Rectangle? sourceRectangle = null;
            spriteBatch.Draw(this.BackgroundTexture, this.BackgroundDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
            this.TitleText.Draw(spriteBatch, 0.1999f);
            foreach (LabelText text in this.LabelTexts)
            {
                text.Label.Draw(spriteBatch, 0.1999f);
                text.Text.Draw(spriteBatch, 0.1999f);
            }
            sourceRectangle = null;
            spriteBatch.Draw(this.DestinationButtonDisplayTexture, this.DestinationButtonDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            sourceRectangle = null;
            spriteBatch.Draw(this.InputNumberButtonDisplayTexture, this.InputNumberButtonDisplayPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            this.DestinationText.Draw(spriteBatch, 0.1999f);
            this.DestinationCommentText.Draw(spriteBatch, 0.1999f);
            this.InputNumberText.Draw(spriteBatch, 0.1999f);
            if (this.StartButtonEnabled)
            {
                spriteBatch.Draw(this.StartButtonDisplayTexture, this.StartButtonDisplayPosition, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            }
            else
            {
                spriteBatch.Draw(this.StartButtonDisabledTexture, this.StartButtonDisplayPosition, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.199f);
            }
        }

        internal void Initialize(Screen screen)
        {
            this.screen = screen;
        }

        private void RefreshStartButton()
        {
            this.StartButtonEnabled = (this.DestinationArchitecture != null) && (this.Number > 0);
        }

        private void screen_OnMouseLeftDown(Point position)
        {
            if (this.screen.PeekUndoneWork().Kind == UndoneWorkKind.Dialog)
            {
                if (StaticMethods.PointInRectangle(position, this.DestinationButtonDisplayPosition))
                {
                    //this.ShowDestinationFrame();
                }
                else if (StaticMethods.PointInRectangle(position, this.InputNumberButtonDisplayPosition))
                {
                    this.ShowInputNumberDialog();
                }
                else if (StaticMethods.PointInRectangle(position, this.StartButtonDisplayPosition) && this.StartButtonEnabled)
                {
                    this.StartTransport();
                }
            }
        }

        private void screen_OnMouseMove(Point position, bool leftDown)
        {
            if (this.screen.PeekUndoneWork().Kind == UndoneWorkKind.Dialog)
            {
                if (StaticMethods.PointInRectangle(position, this.DestinationButtonDisplayPosition))
                {
                    this.DestinationButtonDisplayTexture = this.DestinationButtonSelectedTexture;
                    this.InputNumberButtonDisplayTexture = this.InputNumberButtonTexture;
                    if (this.StartButtonEnabled)
                    {
                        this.StartButtonDisplayTexture = this.StartButtonTexture;
                    }
                }
                else if (StaticMethods.PointInRectangle(position, this.InputNumberButtonDisplayPosition))
                {
                    this.DestinationButtonDisplayTexture = this.DestinationButtonTexture;
                    this.InputNumberButtonDisplayTexture = this.InputNumberButtonSelectedTexture;
                    if (this.StartButtonEnabled)
                    {
                        this.StartButtonDisplayTexture = this.StartButtonTexture;
                    }
                }
                else if (StaticMethods.PointInRectangle(position, this.StartButtonDisplayPosition))
                {
                    this.DestinationButtonDisplayTexture = this.DestinationButtonTexture;
                    this.InputNumberButtonDisplayTexture = this.InputNumberButtonTexture;
                    if (this.StartButtonEnabled)
                    {
                        this.StartButtonDisplayTexture = this.StartButtonSelectedTexture;
                    }
                }
                else
                {
                    this.DestinationButtonDisplayTexture = this.DestinationButtonTexture;
                    this.InputNumberButtonDisplayTexture = this.InputNumberButtonTexture;
                    if (this.StartButtonEnabled)
                    {
                        this.StartButtonDisplayTexture = this.StartButtonTexture;
                    }
                }
            }
        }

        private void screen_OnMouseRightDown(Point position)
        {
            if (this.screen.PeekUndoneWork().Kind == UndoneWorkKind.Dialog)
            {
                this.IsShowing = false;
            }
        }

        internal void SetDisplayOffset(ShowPosition showPosition)
        {
            Rectangle rectDes = new Rectangle(0, 0, this.screen.viewportSize.X, this.screen.viewportSize.Y);
            Rectangle rect = new Rectangle(0, 0, this.BackgroundSize.X, this.BackgroundSize.Y);
            switch (showPosition)
            {
                case ShowPosition.Center:
                    rect = StaticMethods.GetCenterRectangle(rectDes, rect);
                    break;

                case ShowPosition.Top:
                    rect = StaticMethods.GetTopRectangle(rectDes, rect);
                    break;

                case ShowPosition.Left:
                    rect = StaticMethods.GetLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.Right:
                    rect = StaticMethods.GetRightRectangle(rectDes, rect);
                    break;

                case ShowPosition.Bottom:
                    rect = StaticMethods.GetBottomRectangle(rectDes, rect);
                    break;

                case ShowPosition.TopLeft:
                    rect = StaticMethods.GetTopLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.TopRight:
                    rect = StaticMethods.GetTopRightRectangle(rectDes, rect);
                    break;

                case ShowPosition.BottomLeft:
                    rect = StaticMethods.GetBottomLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.BottomRight:
                    rect = StaticMethods.GetBottomRightRectangle(rectDes, rect);
                    break;
            }
            this.DisplayOffset = new Point(rect.X, rect.Y);
            this.TitleText.DisplayOffset = this.DisplayOffset;
            foreach (LabelText text in this.LabelTexts)
            {
                text.Label.DisplayOffset = this.DisplayOffset;
                text.Text.DisplayOffset = this.DisplayOffset;
            }
            this.DestinationText.DisplayOffset = this.DisplayOffset;
            this.DestinationCommentText.DisplayOffset = this.DisplayOffset;
            this.InputNumberText.DisplayOffset = this.DisplayOffset;
        }

        internal void SetKind(TransportKind kind)
        {
            this.Kind = kind;
            switch (kind)
            {
                case TransportKind.Fund:
                    this.TitleText.Text = "进贡资金";

                    if (this.SourceArchitecture.jingongjianzhuliebiao()!=null )
                    {
                        this.DestinationArchitecture = this.SourceArchitecture.jingongjianzhuliebiao()[0] as Architecture;
                    }
                    break;

                case TransportKind.Food:
                    this.TitleText.Text = "进贡粮草";
                    if (this.SourceArchitecture.jingongjianzhuliebiao() != null)
                    {
                        this.DestinationArchitecture = this.SourceArchitecture.jingongjianzhuliebiao()[0] as Architecture;

                    }
                    break;
            }
        }

        internal void SetSourceArchiecture(Architecture architecture)
        {
            this.SourceArchitecture = architecture;
            foreach (LabelText text in this.LabelTexts)
            {
                text.Text.Text = StaticMethods.GetPropertyValue(architecture, text.PropertyName).ToString();
            }
        }

        private void ShowDestinationFrame() //代码未用
        {
            switch (this.Kind)
            {
                case TransportKind.Fund:
                    this.TabListPlugin.InitialValues(this.SourceArchitecture.jingongjianzhuliebiao(), null, 0, this.TitleText.Text);
                    break;

                case TransportKind.Food:
                    this.TabListPlugin.InitialValues(this.SourceArchitecture.RoutewayDestinationArchitectureList, null, 0, this.TitleText.Text);
                    break;
            }
            this.TabListPlugin.SetListKindByName("Architecture", true, false);
            this.TabListPlugin.SetSelectedTab("");
            this.GameFramePlugin.Kind = FrameKind.Architecture;
            this.GameFramePlugin.Function = FrameFunction.GetFacilityToBuild;  //???????可能有BUG。
            this.GameFramePlugin.SetFrameContent(this.TabListPlugin.TabList, this.screen.viewportSize);
            this.GameFramePlugin.OKButtonEnabled = false;
            this.GameFramePlugin.CancelButtonEnabled = true;
            this.GameFramePlugin.SetOKFunction(delegate {
                this.DestinationArchitecture = this.TabListPlugin.SelectedItem as Architecture;
            });
            this.GameFramePlugin.IsShowing = true;
        }

        private void ShowInputNumberDialog()
        {
            switch (this.Kind)
            {
                case TransportKind.Fund:
                    this.NumberInputerPlugin.SetMax(this.SourceArchitecture.Fund);
                    break;

                case TransportKind.Food:
                    this.NumberInputerPlugin.SetMax(this.SourceArchitecture.Food);
                    break;
            }
            this.NumberInputerPlugin.SetDepthOffset(-0.01f);
            this.NumberInputerPlugin.SetMapPosition(ShowPosition.Center);
            this.NumberInputerPlugin.SetEnterFunction(delegate {
                this.Number = this.NumberInputerPlugin.Number;
            });
            this.NumberInputerPlugin.IsShowing = true;
        }

        private void StartTransport()
        {
            Faction faction = this.SourceArchitecture.BelongedFaction;
            switch (this.Kind)
            {
                case TransportKind.Fund:
                    this.SourceArchitecture.DecreaseFund(this.Number);
                    if (this.screen.Scenario.huangdisuozaijianzhu().BelongedFaction != this.SourceArchitecture.BelongedFaction)
                    {
                        this.DestinationArchitecture.AddFundPack(this.Number, this.Days);
                    }
                    this.SourceArchitecture.BelongedFaction.chaotinggongxiandu += this.Number;
                    break;

                case TransportKind.Food:
                    this.SourceArchitecture.DecreaseFood(this.Number);
                    if (this.screen.Scenario.huangdisuozaijianzhu().BelongedFaction != this.SourceArchitecture.BelongedFaction)
                    {
                        this.DestinationArchitecture.IncreaseFood(this.Number);
                    }
                    this.SourceArchitecture.BelongedFaction.chaotinggongxiandu += this.Number/200;

                    break;
            }

            faction.TextResultString = this.Number.ToString();
            faction.TextDestinationString = (this.Kind==TransportKind.Fund)?"资金":"粮草";

            this.GameRecordPlugin.AddBranch(faction, "shilijingong", faction.Capital.Position);


            this.IsShowing = false;
        }

        internal void Update()
        {
        }

        private Rectangle BackgroundDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X, this.DisplayOffset.Y, this.BackgroundSize.X, this.BackgroundSize.Y);
            }
        }

        internal Architecture DestinationArchitecture
        {
            get
            {
                return this.destinationArchitecture;
            }
            set
            {
                this.destinationArchitecture = value;
                if (this.destinationArchitecture != null)
                {
                    this.DestinationText.Text = this.destinationArchitecture.Name;
                    switch (this.Kind)
                    {
                        case TransportKind.Fund:
                            //this.Days = this.screen.Scenario.GetTranferFundDays(this.SourceArchitecture, this.DestinationArchitecture);
                            this.Days = 1;
                            //this.DestinationCommentText.Text = "运抵时间：" + this.Days.ToString() + "天";
                            this.DestinationCommentText.Text = "";
                            break ;

                        case TransportKind.Food:
                            //this.SurplusRate = this.DestinationArchitecture.CurrentSurplusRate;
                            //this.DestinationCommentText.Text = "粮草运抵剩余率：" + StaticMethods.GetPercentString(this.SurplusRate, 1);
                            this.DestinationCommentText.Text = "";
                            break ;
                    }
                }
                else
                {
                    this.DestinationText.Text = "----";
                    this.DestinationCommentText.Text = "";
                }
            //Label_00ED:
                this.RefreshStartButton();
            }
        }

        private Rectangle DestinationButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X + this.DestinationButtonPosition.X, this.DisplayOffset.Y + this.DestinationButtonPosition.Y, this.DestinationButtonPosition.Width, this.DestinationButtonPosition.Height);
            }
        }

        private Rectangle InputNumberButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X + this.InputNumberButtonPosition.X, this.DisplayOffset.Y + this.InputNumberButtonPosition.Y, this.InputNumberButtonPosition.Width, this.InputNumberButtonPosition.Height);
            }
        }

        public bool IsShowing
        {
            get
            {
                return this.isShowing;
            }
            set
            {
                this.isShowing = value;
                if (value)
                {
                    this.screen.PushUndoneWork(new UndoneWorkItem(UndoneWorkKind.Dialog, UndoneWorkSubKind.None));
                    this.screen.OnMouseLeftDown += new Screen.MouseLeftDown(this.screen_OnMouseLeftDown);
                    this.screen.OnMouseMove += new Screen.MouseMove(this.screen_OnMouseMove);
                    this.screen.OnMouseRightDown += new Screen.MouseRightDown(this.screen_OnMouseRightDown);
                }
                else
                {
                    if (this.screen.PopUndoneWork().Kind != UndoneWorkKind.Dialog)
                    {
                        throw new Exception("The UndoneWork is not a TransportDialog.");
                    }
                    this.screen.OnMouseLeftDown -= new Screen.MouseLeftDown(this.screen_OnMouseLeftDown);
                    this.screen.OnMouseMove -= new Screen.MouseMove(this.screen_OnMouseMove);
                    this.screen.OnMouseRightDown -= new Screen.MouseRightDown(this.screen_OnMouseRightDown);
                    this.DestinationArchitecture = null;
                    this.Number = 0;
                }
            }
        }

        internal int Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
                this.InputNumberText.Text = this.number.ToString();
                this.RefreshStartButton();
            }
        }

        private Rectangle StartButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X + this.StartButtonPosition.X, this.DisplayOffset.Y + this.StartButtonPosition.Y, this.StartButtonPosition.Width, this.StartButtonPosition.Height);
            }
        }
    }
}

