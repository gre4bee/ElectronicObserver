﻿using Browser.Properties;
using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	public class CustomRequestHandler : RequestHandler
	{
		public delegate void RenderProcessTerminatedEventHandler(string message);
		public event RenderProcessTerminatedEventHandler RenderProcessTerminated;

		bool pixiSettingEnabled;



		public CustomRequestHandler(bool pixiSettingEnabled) : base()
		{
			this.pixiSettingEnabled = pixiSettingEnabled;
		}

		/// <summary>
		/// 戻る/進む操作をブロックします。
		/// </summary>
		protected override bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
		{
			if ((request.TransitionType & TransitionType.ForwardBack) != 0)
			{
				return true;
			}
			return base.OnBeforeBrowse(browserControl, browser, frame, request, userGesture, isRedirect);
		}

		/// <summary>
		/// 描画プロセスが何らかの理由で落ちた際の処理を行います。
		/// </summary>
		protected override void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
		{
			// note: out of memory (例外コード: 0xe0000008) でクラッシュした場合、このイベントは呼ばれない

			string ret = Resources.RenderProcessTerminatedBy;
			switch (status)
			{
				case CefTerminationStatus.AbnormalTermination:
					ret += Resources.RenderProcessAbnormalTermination;
					break;
				case CefTerminationStatus.ProcessWasKilled:
					ret += Resources.RenderProcessProcessWasKilled;
					break;
				case CefTerminationStatus.ProcessCrashed:
					ret += Resources.RenderProcessProcessCrashed;
					break;
				default:
					ret += Resources.RenderProcessUnexpectedTermination;
					break;
			}
			ret += Resources.RenderProcessReturnWhenReloaded;

			RenderProcessTerminated(ret);
		}

		/*protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
		{
			return new CustomResourceRequestHandler(pixiSettingEnabled);
		}*/

		protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool iNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
		{
			//NOTE: In most cases you examine the request.Url and only handle requests you are interested in
			if (request.Url.Contains(@"/kcs2/resources/bgm/"))
			{
				return new ResRequestHandler();
			}
			if (request.Url.Contains("gadget_html5"))
				return new GadgetUrlHandler();

			return new CustomResourceRequestHandler(pixiSettingEnabled);
		}
	}
}
