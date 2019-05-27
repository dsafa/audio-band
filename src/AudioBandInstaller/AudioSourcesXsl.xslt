<?xml version="1.0" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:wix="http://schemas.microsoft.com/wix/2006/wi">

  <!-- Copy all attributes and elements to the output. -->
  <xsl:template match="@*|*">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:apply-templates select="*" />
    </xsl:copy>
  </xsl:template>

  <xsl:output method="xml" indent="yes" />

  <!-- Key for AudioBand.AudioSource library files -->
  <xsl:key name="audiosourcelib-component" match="wix:Component[wix:File[contains(@Source, 'AudioBand.AudioSource')]]" use="@Id"/>

  <!-- Remove components containing audiosource library files-->
  <xsl:template match="wix:Component[key('audiosourcelib-component', @Id)]"/>
  <xsl:template match="wix:ComponentRef[key('audiosourcelib-component', @Id)]"/>
</xsl:stylesheet>