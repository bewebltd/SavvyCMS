/**
 * $Id: editor_plugin_src.js 539 2011-01-10 19:08:58Z spocke $
 *
 * @author Beweb
 * @copyright Copyright © 2010, Beweb ltd, All rights reserved.
 */

(function() {
	tinymce.create('tinymce.plugins.BewebYoutubePlugin', {
		init : function(ed, url) {
			this.editor = ed;

			// Register commands
			ed.addCommand('mceBewebYoutube', function() {
				var se = ed.selection;

				// No selection and not in link
				//if (se.isCollapsed() && !ed.dom.getParent(se.getNode(), 'A'))
				//	return;

				ed.windowManager.open({
					file : url + '/youtube.aspx',
					width : 780 + parseInt(ed.getLang('advlink.delta_width', 0)),
					height : 800 + parseInt(ed.getLang('advlink.delta_height', 0)),
					inline : 1
				}, {
					plugin_url : url
				});
			});

			// Register buttons
			ed.addButton('youtube', {
				title: 'Youtube.Youtube_desc',
				cmd: 'mceBewebYoutube'
			});

			ed.addShortcut('ctrl+f', 'Youtube.link_desc', 'mceBewebYoutube');

			ed.onNodeChange.add(function (ed, cm, n, co) {
				// see if this is an existing Youtube or a different kind of link
				var isYoutube = n.nodeName == 'A' && (n.href + "").indexOf("Youtubes/docs/") > -1
				cm.setDisabled('youtube', n.nodeName == 'A' && !isYoutube);
				cm.setActive('youtube', isYoutube);
			});
		},

		getInfo : function() {
			return {
				longname : 'youtube',
				author : 'Beweb Ltd',
				authorurl : 'http://www.beweb.co.nz',
				infourl: 'http://www.beweb.co.nz',
				version : tinymce.majorVersion + "." + tinymce.minorVersion
			};
		}
	});

	// Register plugin
	tinymce.PluginManager.add('youtube', tinymce.plugins.BewebYoutubePlugin);
})();