<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	
	<xsl:output indent="yes" omit-xml-declaration="yes"/>

	<!-- NOTE: no group record (GroupRec) equivalent in 5.5 -->
	<!-- NOTE: embedded multimedia records (OBJE) dropped -->


	<!-- save global xml:lang setting for future use -->
	<xsl:variable name="lang" select="string(/GEDCOM/HEAD/LANG/@value)"/>
	
	<xsl:template match="GEDCOM">
		<!-- write DOCTYPE declaration so we can validate output file -->
		<xsl:text disable-output-escaping="yes">&lt;!DOCTYPE GEDCOM SYSTEM "GEDCOMv60.dtd"&gt;</xsl:text>
		<xsl:processing-instruction name="xml-stylesheet">type="text/xsl" href="collapsed.xsl"</xsl:processing-instruction>
		<GEDCOM>
			<!-- GEDCOM header -->
			<xsl:apply-templates select="HEAD"/>
			<!-- family records -->
			<xsl:apply-templates select="FAM"/>
			<!-- individual records -->
			<xsl:apply-templates select="INDI"/>
			<!-- generic individual and family event records -->
			<xsl:apply-templates select="INDI/EVEN | FAM/EVEN"/>
			<!-- specific family event records -->
			<xsl:apply-templates select="FAM/ANUL|FAM/CENS|FAM/DIV|FAM/DIVF|FAM/ENGA|FAM/MARR|FAM/MARB|FAM/MARC|FAM/MARL|FAM/MARS"/>
			<!-- specific individual event records -->
			<xsl:apply-templates select="INDI/BIRT|INDI/CHR|INDI/DEAT|INDI/BURI|INDI/CREM|INDI/ADOP|INDI/BAPM|INDI/BARM|INDI/BASM|INDI/BLES|INDI/CHRA|INDI/CONF|INDI/FCOM|INDI/ORDN|INDI/NATU|INDI/EMIG|INDI/IMMI|INDI/CENS|INDI/PROB|INDI/WILL|INDI/GRAD|INDI/RETI"/>
			<!-- ordinance records -->
			<xsl:apply-templates select="INDI/BAPL|INDI/CONL|INDI/ENDL|INDI/SLGC|FAM/SLGS"/> 
			<!-- contact records -->
			<xsl:apply-templates select="//SUBM[@id] | HEAD/SOUR/CORP"/>
			<!-- source records-->			
			<xsl:apply-templates select="SOUR"/>
			<!-- repository records -->
			<xsl:apply-templates select="REPO"/>
		</GEDCOM>
	</xsl:template>

	<xsl:template match="HEAD">
		<HeaderRec>
			<FileCreation Date="{DATE/@value}" Time="{DATE/TIME/@value}">
				<Product> 
					<ProductId><xsl:value-of select="SOUR/@value"/></ProductId>
					<Version><xsl:value-of select="SOUR/VERS/@value"/></Version>
					<Name>
						<xsl:call-template name="OutputLang"/>
						<xsl:value-of select="SOUR/NAME/@value"/>
					</Name>
					<Supplier>
						<Link Target="ContactRec" Ref="{generate-id(SOUR/CORP)}"/>
					</Supplier> 
				</Product> 
				<Copyright><xsl:value-of select="SOUR/COPR/@value"/></Copyright>
			</FileCreation> 
			<!-- TODO: do we need <Citation> when starting from GEDCOM 5.5? -->
			<xsl:call-template name="TransformSubmitterLink"/>
			<xsl:call-template name="TransformNote"/>
		</HeaderRec>		
	</xsl:template>
	
	<xsl:template match="SUBM">
		<ContactRec Id="{@id}" Type="person">
			<Name>
				<xsl:call-template name="OutputLang"/>
				<xsl:value-of select="NAME/@value"/>	
			</Name>
			<xsl:call-template name="TransformAddress"/>
			<xsl:if test="PHON">
				<!-- TODO: how do we know the type? -->
				<Phone Type="home"><xsl:value-of select="PHON/@value"/></Phone>
			</xsl:if>
			<!-- TODO: is _EMAIL a standard extension? -->
			<xsl:if test="_EMAIL">
				<Email><xsl:value-of select="_EMAIL/@value"/></Email>
			</xsl:if>			
		</ContactRec>	
	</xsl:template>
	
	<xsl:template match="CORP">
		<ContactRec Id="{generate-id(.)}" Type="business">
			<Name>
				<xsl:call-template name="OutputLang"/>
				<xsl:value-of select="@value"/>
			</Name>
			<xsl:call-template name="TransformAddress"/>
		</ContactRec>
	</xsl:template>
		
	<xsl:template match="INDI">
		<IndividualRec Id="{@id}">
			<IndivName>
				<xsl:call-template name="OutputLang"/>
				<!-- TODO: deal with NamePart's and IndNameVariation's ? -->
				<xsl:value-of select="NAME/@value"/>
			</IndivName> 
			<Gender><xsl:value-of select="SEX/@value"/></Gender> 
			<!-- TODO: possible values include 'dead', 'stillborn', 'infant', 'child' -->
			<xsl:if test="DEAT|BURI|CREM">
				<DeathStatus>dead</DeathStatus> 		
			</xsl:if>
			<xsl:call-template name="TransformAttributes"/>
			<xsl:for-each select="ASSO">
				<AssocIndiv>
					<Link Target="IndividualRec" Ref="{@idref}">
						<xsl:choose>
							<xsl:when test="TYPE='FAM'">
								<xsl:attribute name="Target">FamilyRec</xsl:attribute>
							</xsl:when> 
							<xsl:when test="TYPE='INDI'">
								<xsl:attribute name="Target">IndividualRec</xsl:attribute>
							</xsl:when> 
							<xsl:when test="TYPE='REPO'">
								<xsl:attribute name="Target">RepositoryRec</xsl:attribute>
							</xsl:when> 
							<xsl:when test="TYPE='SOUR'">
								<xsl:attribute name="Target">SourceRec</xsl:attribute>
							</xsl:when> 
							<xsl:when test="TYPE='SUBM'">
								<xsl:attribute name="Target">ContactRec</xsl:attribute>
							</xsl:when> 
						</xsl:choose>
					</Link> 
					<Association><xsl:value-of select="RELA/@value"/></Association> 
					<xsl:call-template name="TransformNote"/>
					<!-- TODO: deal with Citation here ? -->			
				</AssocIndiv>
			</xsl:for-each>
			<!-- TODO: is there anything to transform for DupIndiv's ? -->
			<xsl:call-template name="TransformRecordCom"/>
		</IndividualRec>
	</xsl:template>
	
	<xsl:template match="FAM">
		<FamilyRec Id="{@id}">
			<xsl:if test="HUSB">
				<HusbFath>
					<Link Target="IndividualRec" Ref="{HUSB/@idref}"/> 
				</HusbFath> 
			</xsl:if>
			<xsl:if test="WIFE">
				<WifeMoth>
					<Link Target="IndividualRec" Ref="{WIFE/@idref}"/> 
				</WifeMoth> 
			</xsl:if>
			<xsl:for-each select="CHIL">
				<Child>
					<Link Target="IndividualRec" Ref="{@idref}"/>
					<!-- Child's order in family, if birth dates unknown. --> 
					<ChildNbr><xsl:value-of select="position()"/></ChildNbr>
				</Child> 			
			</xsl:for-each>		
			<BasedOn> 
				<!-- all local family events -->
				<xsl:for-each select="ANUL|CENS|DIV|DIVF|ENGA|MARR|MARB|MARC|MARL|MARS">
					<Link Target="EventRec" Ref="{generate-id(.)}"/>
				</xsl:for-each>
				<!-- TODO: identify individual events for family members, just do BIRT and ADOP -->
				<xsl:for-each select="//INDI/BIRT[../FAMC/@idref=current()/@id] | //INDI/ADOP[../FAMC/@idref=current()/@id]">
					<Link Target="EventRec" Ref="{generate-id(.)}"/>				
				</xsl:for-each>
			</BasedOn>
			<xsl:call-template name="TransformRecordCom"/>
		</FamilyRec>
	</xsl:template>
			
	<!-- source records -->
	<!-- 
	  <!ELEMENT SourceRec
		(Repository*,
		Title,
		Article?,
		Author?,
		URI*,
		Publishing?,
		Note*,
		Changed*)>
	-->
	<xsl:template match="SOUR">
		<SourceRec Id="{@id}"> <!-- TODO: do we know the Type? -->
			<xsl:call-template name="OutputLang"/>
			<xsl:for-each select="REPO">
				<Repository>
					<Link Target="RepositoryRec" Ref="{@idref}"/>
					<xsl:if test="CALN">
						<CallNbr><xsl:value-of select="CALN/@value"/></CallNbr>
					</xsl:if>
				</Repository>	
			</xsl:for-each>
			<Title>
				<xsl:value-of select="TITL/@value"/>
				<xsl:call-template name="OutputContinuations"/>
			</Title>	
			<!-- TODO: Article? -->		
			<xsl:if test="AUTH">
				<Author>
					<xsl:value-of select="AUTH/@value"/>
					<xsl:call-template name="OutputContinuations"/>				
				</Author>
			</xsl:if>
			<!-- TODO: we drop cross-referenced multimedia records? -->
			<xsl:for-each select="OBJE/FILE">
				<URI><xsl:value-of select="@value"/></URI>
			</xsl:for-each>			
			<xsl:if test="PUBL">
				<Publishing>
					<xsl:value-of select="AUTH/@value"/>
					<xsl:call-template name="OutputContinuations"/>							
				</Publishing>
			</xsl:if>
			<xsl:call-template name="TransformNote"/>
			<xsl:call-template name="TransformChanged"/>
		</SourceRec>	
	</xsl:template>	
	
	<!-- repository record 
		<!ELEMENT RepositoryRec
			(Name,
			MailAddress*,
			Phone*,
			Email*,
			URI*,
			Note*,
			Changed*)>
	-->	
	<xsl:template match="REPO">
		<RepositoryRec Id="{@id}"> <!-- TODO: do we know the type? -->
			<Name><xsl:value-of select="NAME/@value"/></Name>
			<xsl:call-template name="TransformAddress"/>
			<xsl:call-template name="TransformNote"/>
			<xsl:call-template name="TransformChanged"/>
		</RepositoryRec>
	</xsl:template>	
	
	<!-- generic events -->
		
	<xsl:template match="EVEN">
		<!--
		<EventRec Id="" Type="" VitalType="birth | marriage | death">
			<Participant></Participant> +
			<Date></Date> ?
			<Place></Place> ?
			<Religion></Religion> ?
			%RecordCom;
		</EventRec>
		-->
		<!-- TODO: is it possible to know the VitalType for a generic EVEN ? -->
		<EventRec Id="{generate-id(.)}" Type="{TYPE/@value}">
			<xsl:choose>
				<xsl:when test="parent::INDI">
					<Participant>
						<Link Target="IndividualRec" Ref="{parent::INDI/@id}"/> 
						<Role>principle</Role>
						<xsl:call-template name="LivingStatus"/>
						<xsl:call-template name="AgeAtEvent"/>
					</Participant>
				</xsl:when>
				<xsl:when test="parent::FAM">
					<Participant>
						<Link Target="FamilyRec" Ref="{parent::FAM/@id}"/> 
						<Role>principle</Role> 
						<xsl:call-template name="LivingStatus"/>
						<xsl:call-template name="AgeAtEvent"/>
					</Participant>
				</xsl:when>
			</xsl:choose>
			<xsl:call-template name="TransformDatePlace"/>
			<!-- TODO: Religion is missing from 5.5 ? -->
			<xsl:call-template name="TransformRecordCom"/>
		</EventRec>
	</xsl:template>	
	
	
	<!-- specific family events -->
	
	<!-- TODO: need to output the VitalType attribute for each specific event below -->
	
	<!-- ANUL|CENS|DIV|DIVF|ENGA|MARR|MARB|MARC|MARL|MARS -->
    
	<xsl:template name="OutputHWParticipants">
		<Participant>
			<Link Target="IndividualRec" Ref="{parent::FAM/HUSB/@idref}"/> 
			<Role>husband</Role> 
			<xsl:call-template name="LivingStatus"/>
			<xsl:call-template name="AgeAtEvent"/>
		</Participant>
		<Participant>
			<Link Target="IndividualRec" Ref="{parent::FAM/WIFE/@idref}"/> 
			<Role>wife</Role> 
			<xsl:call-template name="LivingStatus"/>
			<xsl:call-template name="AgeAtEvent"/>
		</Participant>	
	</xsl:template>
	
	<xsl:template name="OutputFamParticipant">
		<Participant>
			<Link Target="FamilyRec" Ref="{parent::FAM/@id}"/> 
			<Role>principle</Role> 
			<xsl:call-template name="LivingStatus"/>
			<xsl:call-template name="AgeAtEvent"/>
		</Participant>	
	</xsl:template>
	
	<xsl:template name="OutputIndParticipant">
		<Participant>
			<Link Target="IndividualRec" Ref="{parent::INDI/@id}"/> 
			<Role>principle</Role> 
			<xsl:call-template name="LivingStatus"/>
			<xsl:call-template name="AgeAtEvent"/>
		</Participant>	
	</xsl:template>
	
	<xsl:template name="OutputEvent">
		<xsl:param name="type"/>
		<xsl:param name="part" select="'ind'"/>
		<EventRec Id="{generate-id(.)}" Type="{$type}">
			<xsl:choose>
				<xsl:when test="$part='hw'">
					<xsl:call-template name="OutputHWParticipants"/>
				</xsl:when>
				<xsl:when test="$part='fam'">
					<xsl:call-template name="OutputFamParticipant"/>				
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="OutputIndParticipant"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:call-template name="TransformDatePlace"/>
			<xsl:call-template name="TransformRecordCom"/>
		</EventRec>				
	</xsl:template>
	
	<xsl:template match="ANUL">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'annulment'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="FAM/CENS">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'census'"/>
			<xsl:with-param name="part" select="'fam'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="DIV">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'divorce'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="DIVF">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'divorce filed'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="ENGA">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'engagement'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="MARR">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'marriage'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="MARB">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'marriage banns'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="MARC">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'marriage contract'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="MARL">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'marriage license'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="MARS">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'marriage settlement'"/>
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>

	<!-- specific individual events -->
	
	<!-- BIRT|CHR|DEAT|BURI|CREM|ADOP|BAPM|BARM|BASM|BLES|CHRA|CONF|FCOM|ORDN|NATU|EMIG|IMMI|CENS|PROB|WILL|GRAD|RETI -->

	<xsl:template match="BIRT">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'birth'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="CHR">
		<!-- TODO: should we output the parents as participants also? -->
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'christening'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="DEAT">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'death'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="BURI">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'burial'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="CREM">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'cremation'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="ADOP">
		<!-- TODO: participants ? -->
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'adoption'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="BAPM">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'baptism'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="BAPL">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'baptism-LDS'"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="BARM">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'bar mitzvah'"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="BASM">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'bas mitzvah'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="BLES">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'blessing'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="CHRA">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'christening-adult'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="CONF">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'confirmation'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="CONL">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'confirmation-LDS'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="FCOM">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'first communion'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="ORDN">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'ordination'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="NATU">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'naturalization'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="EMIG">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'emigration'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="IMMI">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'immigration'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="INDI/CENS">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'census'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="PROB">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'probate'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="WILL">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'will'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="GRAD">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'graduation'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="RETI">
		<xsl:call-template name="OutputEvent">
			<xsl:with-param name="type" select="'retirement'"/>
		</xsl:call-template>
	</xsl:template>

	<!-- ordinance templates -->

	<xsl:template name="LowerCase">
		<xsl:param name="input"/>
		<xsl:value-of select="translate($input, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')"/>
	</xsl:template>	
	
	<xsl:template name="OutputOrdStatus">
		<xsl:if test="STAT">
			<OrdStat>
				<xsl:attribute name="Code">
					<xsl:call-template name="LowerCase">
						<xsl:with-param name="input" select="CODE/@value"/>
					</xsl:call-template>
				</xsl:attribute>
				<Date><xsl:value-of select="DATE/@value"/></Date>	
			</OrdStat>
		</xsl:if>
	</xsl:template>

	<xsl:template name="LivingStatus">
		<!-- TODO: can we determine if a person was living when the event occurred? -->
	</xsl:template>
	
	<xsl:template name="AgeAtEvent">
		<!-- TODO: can we determine the age of an individual when as event occurred? -->
	</xsl:template>

	<!-- 
	<!ELEMENT LDSOrdRec
		(Participant+,
		OrdStat*,
		TempleCode?,
		Date?,
		Place?,
		BasedOn?,
		%RecordCom;)>
	-->	
	<xsl:template name="OutputOrdinance">
		<xsl:param name="type"/>
		<xsl:param name="part" select="'ind'"/>
		<LDSOrdRec Id="{generate-id(.)}" Type="{$type}">
			<xsl:choose>
				<xsl:when test="$part='fmc'">
					<Participant>
						<Link Target="IndividualRec" Ref="{/GEDCOM/FAM[CHIL/@idref = current()/parent::INDI/@id]/HUSB/@idref}"/> 
						<Role>father</Role>
						<xsl:call-template name="LivingStatus"/>
					</Participant>
					<Participant>
						<Link Target="IndividualRec" Ref="{/GEDCOM/FAM[CHIL/@idref = current()/parent::INDI/@id]/WIFE/@idref}"/> 
						<Role>mother</Role>
						<xsl:call-template name="LivingStatus"/>
					</Participant>
					<Participant>
						<Link Target="IndividualRec" Ref="{parent::INDI/@id}"/> 
						<Role>child</Role>
						<xsl:call-template name="LivingStatus"/>
					</Participant>				
				</xsl:when>
				<xsl:when test="$part='hw'">
					<Participant>
						<Link Target="IndividualRec" Ref="{parent::FAM/HUSB/@idref}"/> 
						<Role>husband</Role>
						<xsl:call-template name="LivingStatus"/>
					</Participant>
					<Participant>
						<Link Target="IndividualRec" Ref="{parent::FAM/WIFE/@idref}"/> 
						<Role>wife</Role>
						<xsl:call-template name="LivingStatus"/>
					</Participant>
					<Participant>
						<Link Target="IndividualRec" Ref="{parent::INDI/@id}"/> 
						<Role>child</Role>
						<xsl:call-template name="LivingStatus"/>
					</Participant>				
				</xsl:when>
				<xsl:otherwise>
					<Participant>
						<Link Target="IndividualRec" Ref="{parent::INDI/@id}"/> 
						<Role>principle</Role>
						<xsl:call-template name="LivingStatus"/>
					</Participant>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:call-template name="OutputOrdStatus"/>				
			<TempleCode><xsl:value-of select="TEMP/@value"/></TempleCode>			
			<xsl:call-template name="TransformDatePlace"/>
			<BasedOn/> 	<!-- TODO -->
			<xsl:call-template name="TransformRecordCom"/>
		</LDSOrdRec>		
	</xsl:template>
	
	<xsl:template match="BAPL">
		<xsl:call-template name="OutputOrdinance">
			<xsl:with-param name="type" select="'B'"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="CONL">
		<xsl:call-template name="OutputOrdinance">
			<xsl:with-param name="type" select="'C'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="ENDL">
		<xsl:call-template name="OutputOrdinance">
			<xsl:with-param name="type" select="'E'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="SLGC">
		<xsl:call-template name="OutputOrdinance">
			<xsl:with-param name="type" select="'SP'"/>
			<xsl:with-param name="part" select="'fmc'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="SLGS">
		<xsl:call-template name="OutputOrdinance">
			<xsl:with-param name="type" select="'SS'"/> <!-- TODO: SM? -->
			<xsl:with-param name="part" select="'hw'"/>
		</xsl:call-template>
	</xsl:template>

	<!-- Reusable named templates -->
	
	<xsl:template name="TransformEventDetail">
	
	</xsl:template>
	
	<xsl:template name="TransformDatePlace">
		<xsl:if test="DATE">
			<Date><xsl:value-of select="DATE/@value"/></Date>
		</xsl:if>
		<xsl:if test="PLAC">
			<Place>
				<PlaceName>
					<xsl:call-template name="OutputLang"/>
					<!-- TODO: deal with PlaceName/PlacePart's ? -->
					<xsl:value-of select="PLAC/@value"/>
				</PlaceName>				
				<!-- TODO: deal with PlaceNameVar's ?-->
			</Place>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="TransformAttributes">
		<!-- attributes -->
		<xsl:for-each select="CAST|DSCR|EDUC|IDNO|NATI|OCCU|PROP|RELI|RESI|SSN|TITL">
			<PersInfo>
				<xsl:choose>
					<xsl:when test="name()='CAST'">
						<xsl:attribute name="Type">cast</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='DSCR'">
						<xsl:attribute name="Type">attribute</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='EDUC'">
						<xsl:attribute name="Type">education</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='IDNO'">
						<xsl:attribute name="Type"><xsl:value-of select="TYPE"/></xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='NATI'">
						<xsl:attribute name="Type">nationality</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='OCCU'">
						<xsl:attribute name="Type">occupation</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='PROP'">
						<xsl:attribute name="Type">property</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='RELI'">
						<xsl:attribute name="Type">religion</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='RESI'">
						<xsl:attribute name="Type">residence</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='SSN'">
						<xsl:attribute name="Type">SSN</xsl:attribute>
					</xsl:when>
					<xsl:when test="name()='TITL'">
						<xsl:attribute name="Type">title</xsl:attribute>
					</xsl:when>
				</xsl:choose>
				<Information>
					<xsl:value-of select="@value"/>
				</Information>
				<xsl:call-template name="TransformDatePlace"/>
			</PersInfo>		
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template name="TransformAddress">
		<xsl:for-each select="ADDR">
			<MailAddress>
				<xsl:call-template name="OutputLang"/>
				<AddrLine><xsl:value-of select="@value"/></AddrLine>
				<xsl:for-each select="CONT">
					<AddrLine><xsl:value-of select="@value"/></AddrLine>					
				</xsl:for-each>
				<xsl:if test="ADR1">
					<AddrLine><xsl:value-of select="ADR1"/></AddrLine>					
				</xsl:if>
				<xsl:if test="ADR2">
					<AddrLine><xsl:value-of select="ADR2"/></AddrLine>					
				</xsl:if>
				<xsl:if test="CITY|STAE|CTRY">
					<AddrLine>
						<xsl:if test="CITY">
							<PlacePart Level="4" Type="city"><xsl:value-of select="CITY"/></PlacePart>					
						</xsl:if>							
						<xsl:if test="STAE">
							<PlacePart Level="2" Type="state"><xsl:value-of select="STAE"/></PlacePart>					
						</xsl:if>							
						<xsl:if test="CTRY">
							<PlacePart Level="1" Type="country"><xsl:value-of select="CTRY"/></PlacePart>					
						</xsl:if>							
					</AddrLine>											
				</xsl:if>
				<!-- TODO: should we always put POST on a separate line? -->
				<xsl:if test="POST">
					<AddrLine>
						<PlacePart Level="5" Type="postal code"><xsl:value-of select="POST"/></PlacePart>					
					</AddrLine>											
				</xsl:if>
			</MailAddress>	
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="OutputContinuations">
		<xsl:for-each select="CONT|CONC">
			<xsl:value-of select="./@value"/>
		</xsl:for-each>	
		<!-- TODO: worry about newlines for CONT? -->
	</xsl:template>
	
	<xsl:template name="TransformNote">
		<xsl:for-each select="NOTE">
			<Note>
				<xsl:call-template name="OutputLang"/>
				<xsl:value-of select="@value"/>
				<xsl:call-template name="OutputContinuations"/>
			</Note> 		
		</xsl:for-each>
	</xsl:template>	

	<xsl:template name="TransformChanged">
		<xsl:for-each select="CHAN">
			<Changed Date="{DATE/@value}" Time="{DATE/TIME/@value}">
				<!-- TODO: the Contact is the person responsible for the change
				<Contact>
					<Link Target="ContactRec" Ref=". . ."/> 
				</Contact> 
				-->
				<xsl:call-template name="TransformNote"/>
			</Changed> 
		</xsl:for-each>		
	</xsl:template>		
	
	<xsl:template name="TransformExternalIds">
		<!-- TODO: are the Type values always the same as the GEDCOM element names? -->
		<xsl:for-each select="AFN|REFN|RFN|RIN">
			<ExternalID Type="{name()}" Id="{@value}"/>
		</xsl:for-each> 
	</xsl:template>
	
	<xsl:template name="TransformSubmitterLink">
		<xsl:for-each select="SUBM">
			<Submitter>
				<Link Target="ContactRec" Ref="{@idref}"/>
			</Submitter>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template name="TransformEvidence">
		<!-- TODO -->
	</xsl:template>

	<xsl:template name="TransformEnrichment">
		<!-- TODO -->
	</xsl:template>
	
	<xsl:template name="TransformRecordCom">
		<!-- 
		<!ENTITY % RecordCom "
			ExternalID*, 
			Submitter?, 
			Note*, 
			Evidence*, 
			Enrichment*, 
			Changed*">
		-->
		<xsl:call-template name="TransformExternalIds"/>
		<xsl:call-template name="TransformSubmitterLink"/>
		<xsl:call-template name="TransformNote"/>
		<xsl:call-template name="TransformEvidence"/>
		<xsl:call-template name="TransformEnrichment"/>
		<xsl:call-template name="TransformChanged"/>
	</xsl:template>
	
	<xsl:template name="OutputLang">
		<xsl:if test="$lang != ''">
			<xsl:attribute name="xml:lang">
				<!-- TODO: need to fixup language: 'English' to 'en', etc. -->
				<!-- SEE http://www.oasis-open.org/cover/iso639a.html for ISO 639 language codes -->
				<xsl:choose>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='Afrikaans'">af</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:when test="$lang='English'">en</xsl:when>
					<xsl:otherwise>en</xsl:otherwise>
				</xsl:choose>							
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	
</xsl:stylesheet>
