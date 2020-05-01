<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:wix="http://schemas.microsoft.com/wix/2006/wi">
  <xsl:output omit-xml-declaration="yes" indent="yes" />

  <xsl:key name="MainExe" match="wix:Component[contains(wix:File/@Source, 'PixelPalette.exe')]" use="@Id" />

  <!-- By default, copy all elements and nodes into the output... -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <!-- ...but if the element has the "MainExe" key then don't render anything (i.e. removing it from the output) -->
  <xsl:template match="*[ self::wix:Component or self::wix:ComponentRef ][ key( 'MainExe', @Id ) ]" />
</xsl:stylesheet>