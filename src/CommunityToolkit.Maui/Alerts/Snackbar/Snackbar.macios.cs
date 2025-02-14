﻿using System.Runtime.InteropServices;
using CommunityToolkit.Maui.Core.Views;
using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace CommunityToolkit.Maui.Alerts;

public partial class Snackbar
{
	/// <summary>
	/// Dismiss Snackbar
	/// </summary>
	private partial Task DismissPlatform(CancellationToken token)
	{
		if (PlatformSnackbar is not null)
		{
			token.ThrowIfCancellationRequested();
			PlatformSnackbar.Dismiss();
			PlatformSnackbar = null;
		}

		return Task.CompletedTask;
	}

	/// <summary>
	/// Show Snackbar
	/// </summary>
	private async partial Task ShowPlatform(CancellationToken token)
	{
		await DismissPlatform(token);
		token.ThrowIfCancellationRequested();

		var cornerRadius = GetCornerRadius(VisualOptions.CornerRadius);

		var padding = GetMaximum(cornerRadius.X, cornerRadius.Y, cornerRadius.Width, cornerRadius.Height);
		PlatformSnackbar = new PlatformSnackbar(Text,
											VisualOptions.BackgroundColor.ToPlatform(),
											cornerRadius,
											VisualOptions.TextColor.ToPlatform(),
											UIFont.SystemFontOfSize((NFloat)VisualOptions.Font.Size),
											VisualOptions.CharacterSpacing,
											ActionButtonText,
											VisualOptions.ActionButtonTextColor.ToPlatform(),
											UIFont.SystemFontOfSize((NFloat)VisualOptions.ActionButtonFont.Size),
											padding)
		{
			Action = Action,
			Anchor = Anchor?.Handler?.PlatformView as UIView,
			Duration = Duration,
			OnDismissed = OnDismissed,
			OnShown = OnShown
		};

		PlatformSnackbar.Show();

		static T? GetMaximum<T>(params T[] items) => items.Max();
	}

	static CGRect GetCornerRadius(CornerRadius cornerRadius)
	{
		return new CGRect(cornerRadius.BottomLeft, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight);
	}
}