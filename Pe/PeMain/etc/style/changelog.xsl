<?xml version='1.0'?>
<xsl:transform version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html"/>
	
	<xsl:template match="/">
		<html>
			<head>
				<title>Pe更新履歴</title>
				<style>
				@import url('../etc/style/common.css');
				</style>
				<script src="../etc/script/changelog.js"></script>
			</head>
			<body>
				<h1>Pe更新履歴</h1>
				<noscript>
					<p>Issueへのリンクを有効にするにはブロックを解除してください。</p>
				</noscript>
				<xsl:apply-templates />
			</body>
		</html>
	</xsl:template>
	
	<xsl:template match="log">
		<h2>
			<xsl:value-of select="@date" />, <xsl:value-of select="@version" />
			<xsl:if test="@type='rc'"><em>RC版</em></xsl:if>
		</h2>
		<dl class="changelog">
			<xsl:apply-templates />
		</dl>
	
	</xsl:template>
	
	<xsl:template match="log/ul">
		<dt class="{@type}">
			<xsl:choose>
				<xsl:when test="@type='note'">
				メモ
				</xsl:when>
				<xsl:when test="@type='features'">
				機能
				</xsl:when>
				<xsl:when test="@type='fixes'">
				修正
				</xsl:when>
				<xsl:when test="@type='developer'">
				開発
				</xsl:when>
			</xsl:choose>
		</dt>
		<dd class="{@type}">
			<ul>
				<xsl:apply-templates />
			</ul>
		</dd>
	</xsl:template>
	
	<xsl:template match="log/ul/li">
		<xsl:choose>
			<xsl:when test="@class != ''">
				<li class="{@class}">
					<xsl:value-of select="." />。
					<xsl:if test="@rev != ''">
						<a class="rev"><xsl:value-of select="@rev" /></a>
					</xsl:if>
				</li>
			</xsl:when>
			<xsl:otherwise>
				<li>
					<xsl:value-of select="." />。
					<xsl:if test="@rev != ''">
						<a class="rev"><xsl:value-of select="@rev" /></a>
					</xsl:if>
				</li>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:transform>
