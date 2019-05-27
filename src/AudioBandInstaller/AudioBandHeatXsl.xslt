<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl"
                xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
                xmlns:my="my:my">

  <xsl:output method="xml" indent="yes" />

  <xsl:strip-space elements="*"/>

  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:key name="audiobandDllFile" match="wix:Component[wix:File[@Source = '$(var.AudioBandHarvestPath)\AudioBand.dll']]" use="@Id"/>
  <!-- Remove file reference to audioband.dll, it will be added by AudioBandCOM.wxs-->
  <xsl:template match="*[self::wix:Component or self::wix:ComponentRef][key('audiobandDllFile', @Id)]" />
</xsl:stylesheet>