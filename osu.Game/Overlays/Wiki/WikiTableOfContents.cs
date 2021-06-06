// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Containers.Markdown;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;

namespace osu.Game.Overlays.Wiki
{
    public class WikiTableOfContents : CompositeDrawable
    {
        private readonly FillFlowContainer content;

        private Container lastMainTitle;

        private Container lastSubTitle;

        public WikiTableOfContents()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            InternalChild = content = new FillFlowContainer
            {
                Direction = FillDirection.Vertical,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
            };
        }

        public void AddEntry(string title, MarkdownHeading target, bool subtitle = false)
        {
            var entry = new TableOfContentsEntry(title, target, subtitle);

            if (subtitle)
            {
                lastMainTitle.Margin = new MarginPadding(0);

                if (lastSubTitle != null)
                    lastSubTitle.Margin = new MarginPadding(0);

                content.Add(lastSubTitle = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Margin = new MarginPadding { Bottom = 10 },
                    Child = entry,
                });

                return;
            }

            lastSubTitle = null;

            content.Add(lastMainTitle = new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Margin = new MarginPadding { Bottom = 5 },
                Child = entry,
            });
        }

        private class TableOfContentsEntry : OsuHoverContainer
        {
            [Resolved]
            private OverlayColourProvider colourProvider { get; set; }

            private readonly MarkdownHeading target;

            private readonly OsuTextFlowContainer textFlow;

            public TableOfContentsEntry(string text, MarkdownHeading target, bool subtitle = false)
            {
                this.target = target;

                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
                Child = textFlow = new OsuTextFlowContainer(t =>
                {
                    t.Font = OsuFont.GetFont(size: subtitle ? 12 : 15);
                }).With(f =>
                {
                    f.AddText(text);
                    f.RelativeSizeAxes = Axes.X;
                    f.AutoSizeAxes = Axes.Y;
                });
                Margin = new MarginPadding { Bottom = 2 };
                Padding = new MarginPadding { Left = subtitle ? 10 : 0 };
            }

            protected override IEnumerable<Drawable> EffectTargets => new Drawable[] { textFlow };

            [BackgroundDependencyLoader]
            private void load(OverlayScrollContainer scrollContainer)
            {
                IdleColour = colourProvider.Light2;
                HoverColour = colourProvider.Light1;
                Action = () => scrollContainer.ScrollTo(target);
            }
        }
    }
}
