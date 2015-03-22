/**
 * $Id: editor_plugin_src.js 539 2011-01-10 19:08:58Z spocke $
 *
 * @author Beweb
 * @copyright Copyright © 2010, Beweb ltd, All rights reserved.
 */

(function() {
	tinymce.create('tinymce.plugins.BewebMediauploadPlugin', {
		init : function(ed, url) {
			this.editor = ed;

			// Register commands
			ed.addCommand('mceBewebMediaupload', function () {
				var se = ed.selection;

				// No selection and not in link
				//if (se.isCollapsed() && !ed.dom.getParent(se.getNode(), 'A'))
				//	return;

				ed.windowManager.open({
					file : url + '/mediaupload.aspx',
					width : 480 + parseInt(ed.getLang('advlink.delta_width', 0)),
					height : 460 + parseInt(ed.getLang('advlink.delta_height', 0)),
					inline : 1
				}, {
					plugin_url : url
				});
			});

			// Register buttons
			ed.addButton('mediaupload', {
				title: 'mediaupload.mediaupload_desc',
				cmd: 'mceBewebMediaupload'
			});

			ed.addShortcut('ctrl+f', 'mediaupload.link_desc', 'mceBewebMediaupload');

			ed.onNodeChange.add(function (ed, cm, n, co) {
				// see if this is an existing attachment or a different kind of link
				var isAttachment = n.nodeName == 'A' && (n.href + "").indexOf("attachments/docs/") > -1
				cm.setDisabled('attachment', n.nodeName == 'A' && !isAttachment);
				cm.setActive('attachment', isAttachment);
			});
		},

		getInfo : function() {
			return {
				longname : 'Media Upload',
				author : 'Beweb Ltd',
				authorurl : 'http://www.beweb.co.nz',
				infourl: 'http://www.beweb.co.nz',
				version : tinymce.majorVersion + "." + tinymce.minorVersion
			};
		}
	});

	// Register plugin
	tinymce.PluginManager.add('mediaupload', tinymce.plugins.BewebMediauploadPlugin);
})();