<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <head>
        <title><xsl:value-of select="/rss/channel/title" /></title>
        <link rel="stylesheet" type="text/css" href="rss-text.css" />
        <style type="text/css">
          body { font-family: Verdana, Helvetica, sans-serif; font-size: 0.8em; }
          #headerInfo { text-align: center; width:90%; }
          #feedItems { width: 80%; border: 1px solid #333; padding-left: 15px; padding-right: 15px; padding-top: 5px; padding-bottom: 5px; text-align: left; }
          #headerText { text-align: center; padding: 8px; border: 1px dashed #bbb; background-color: #f7f7f7; }
          .rssDescription { padding-left: 25px; }
        </style>
        
        <script type="text/javascript" src="js/rss-disableOutputEscaping.js" />
      </head>
      
      <body onload="go_decoding();">
        <div id="cometestme" style="display:none;">
          <xsl:text disable-output-escaping="yes">&amp;amp;</xsl:text>
        </div>

        <div align="center">
          <div id="headerInfo">
            <h1><xsl:value-of select="/rss/channel/title" /></h1>
            <p align="center" id="headerText">This page is the syndication feed for <b><xsl:value-of select="/rss/channel/title" /></b>.  You can subscribe
            to this feed using an aggregator program and be kept abreast of the latest rishworthaviation.com jobs.  For more information
              check out <a href="http://www.rishworthaviation.com/">Rishworth Aviation</a>.  .</p>
          </div>
        </div>
        <br />
        
        <div align="center">
          <div id="feedItems">
            <xsl:for-each select="/rss/channel/item">
              <div class="rssItem">
                <h2 class="rssTitle"><a href="{link}"><xsl:value-of select="title" /></a></h2>
                <div name="decodeable" class="rssDescription">
                  <xsl:value-of select="description" disable-output-escaping="yes" />
                </div>
              </div>            
            </xsl:for-each>
          </div>
        </div>
      </body>
    </html>
  </xsl:template>
  
</xsl:stylesheet>