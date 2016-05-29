<?xml version='1.0'?>
<xsl:transform version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text"/>
<xsl:template match="*">
  <xsl:apply-templates select="@TEXT | node()"/>
</xsl:template>

<xsl:template match="node/@TEXT | text()">
  <xsl:if test="normalize-space(.)">
    <xsl:value-of select=
     "concat(normalize-space(.), '&#xA;')"/>
  </xsl:if>
  <xsl:apply-templates />
</xsl:template>

<xsl:template match="/">
var chnagelog = [
	<xsl:apply-templates />/*LAST*/
];
</xsl:template>

<xsl:template match="log">
{
	date: '<xsl:value-of select="@date" />',
	version: '<xsl:value-of select="@version" />',
	isRc: <xsl:choose><xsl:when test="@type='rc'">true</xsl:when><xsl:otherwise>false</xsl:otherwise></xsl:choose>,
	contents: [
		<xsl:apply-templates />/*LAST*/
	]
},</xsl:template>

<xsl:template match="log/group">
		{
			type: '<xsl:value-of select="@type" />'
			logs: [
				<xsl:apply-templates />/*LAST*/
			]
		},</xsl:template>

<xsl:template match="log/group/note">
				log: {
					revision: '<xsl:value-of select="@rev" />',
					class: '<xsl:value-of select="message/@class" />',
					subject: '<xsl:value-of select="message" />'<xsl:apply-templates />
				},</xsl:template>

<xsl:template match="log/group/note/message" />

<xsl:template match="log/group/note/comments">,
					comments: [
						<xsl:apply-templates />/*LAST*/
					]
</xsl:template>

<xsl:template match="log/group/note/comments/comment">'<xsl:value-of select="." />',</xsl:template>

</xsl:transform>
