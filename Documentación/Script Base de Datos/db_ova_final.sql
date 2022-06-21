/****** Object:  Table [dbo].[Auditoria]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auditoria](
	[idAuditoria] [int] IDENTITY(1,1) NOT NULL,
	[tabla] [varchar](50) NOT NULL,
	[registro] [varchar](50) NOT NULL,
	[fecha] [datetime] NOT NULL,
	[userName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Auditoria] PRIMARY KEY CLUSTERED 
(
	[idAuditoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[configEmail]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[configEmail](
	[idConfigEmail] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NULL,
	[servidor] [varchar](50) NOT NULL,
	[usuario] [varchar](50) NOT NULL,
	[clave] [varchar](50) NOT NULL,
	[puerto] [int] NOT NULL,
	[ssl] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Evidencias]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Evidencias](
	[idEvidencia] [int] IDENTITY(1,1) NOT NULL,
	[archivo] [varchar](50) NULL,
	[observacion] [varchar](max) NULL,
	[idTema] [int] NOT NULL,
	[idUnidad] [int] NOT NULL,
	[idRecurso] [int] NOT NULL,
	[retroalimentacion] [varchar](max) NULL,
	[puntosAlcanzados] [int] NULL,
	[entregado] [bit] NULL,
	[userName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Evidencias] PRIMARY KEY CLUSTERED 
(
	[idEvidencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NivelEstudio]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NivelEstudio](
	[idNivelEstudios] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](20) NOT NULL,
 CONSTRAINT [PK_NivelEstudio] PRIMARY KEY CLUSTERED 
(
	[idNivelEstudios] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profesores]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profesores](
	[idProfesor] [int] IDENTITY(1,1) NOT NULL,
	[profesion] [varchar](50) NOT NULL,
	[perfilProfesional] [varchar](max) NULL,
	[fotografia] [varchar](max) NULL,
	[userName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Profesores] PRIMARY KEY CLUSTERED 
(
	[idProfesor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recursos]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recursos](
	[idRecurso] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](250) NOT NULL,
	[descripcion] [varchar](max) NULL,
	[url] [varchar](250) NULL,
	[archivo] [varchar](max) NULL,
	[imagen] [varchar](max) NULL,
	[estado] [bit] NOT NULL,
	[idUnidad] [int] NOT NULL,
	[fecha] [datetime] NOT NULL,
	[fechaModifica] [datetime] NULL,
	[userName] [varchar](50) NOT NULL,
	[evidencia] [bit] NULL,
	[descripcionEvidencia] [varchar](250) NULL,
	[puntosRecurso] [int] NULL,
	[limiteEntrega] [datetime] NULL,
	[TipoRecurso] [varchar](1) NOT NULL,
 CONSTRAINT [PK_Recursos] PRIMARY KEY CLUSTERED 
(
	[idRecurso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[idRol] [int] IDENTITY(1,1) NOT NULL,
	[nombreRol] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tema]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tema](
	[idTema] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](max) NOT NULL,
	[descripcion] [varchar](max) NULL,
	[imagen] [varchar](max) NULL,
	[estado] [bit] NOT NULL,
	[fecha] [datetime] NOT NULL,
	[fechaModifica] [datetime] NULL,
	[userName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Tema] PRIMARY KEY CLUSTERED 
(
	[idTema] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Unidades]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Unidades](
	[idUnidad] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](250) NOT NULL,
	[descripcion] [varchar](max) NOT NULL,
	[estado] [bit] NOT NULL,
	[idTema] [int] NOT NULL,
	[fecha] [datetime] NOT NULL,
	[fechaModifica] [datetime] NULL,
	[userName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Unidades] PRIMARY KEY CLUSTERED 
(
	[idUnidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 9/06/2022 9:21:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[idUserName] [int] IDENTITY(1,1) NOT NULL,
	[userName] [varchar](50) NOT NULL,
	[userPassword] [varchar](50) NOT NULL,
	[correoElectronico] [varchar](50) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[apellidos] [varchar](50) NOT NULL,
	[estado] [bit] NOT NULL,
	[idRol] [int] NOT NULL,
	[fecha] [datetime] NOT NULL,
	[fechaModifica] [datetime] NULL,
	[idNivelEstudios] [int] NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[userName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Recursos] ADD  CONSTRAINT [DF_Recursos_TipoRecurso]  DEFAULT ('C') FOR [TipoRecurso]
GO
ALTER TABLE [dbo].[Auditoria]  WITH CHECK ADD  CONSTRAINT [FK_Auditoria_Usuarios] FOREIGN KEY([userName])
REFERENCES [dbo].[Usuarios] ([userName])
GO
ALTER TABLE [dbo].[Auditoria] CHECK CONSTRAINT [FK_Auditoria_Usuarios]
GO
ALTER TABLE [dbo].[Evidencias]  WITH CHECK ADD  CONSTRAINT [FK_Evidencias_Recursos] FOREIGN KEY([idRecurso])
REFERENCES [dbo].[Recursos] ([idRecurso])
GO
ALTER TABLE [dbo].[Evidencias] CHECK CONSTRAINT [FK_Evidencias_Recursos]
GO
ALTER TABLE [dbo].[Evidencias]  WITH CHECK ADD  CONSTRAINT [FK_Evidencias_Tema] FOREIGN KEY([idTema])
REFERENCES [dbo].[Tema] ([idTema])
GO
ALTER TABLE [dbo].[Evidencias] CHECK CONSTRAINT [FK_Evidencias_Tema]
GO
ALTER TABLE [dbo].[Evidencias]  WITH CHECK ADD  CONSTRAINT [FK_Evidencias_Unidades] FOREIGN KEY([idUnidad])
REFERENCES [dbo].[Unidades] ([idUnidad])
GO
ALTER TABLE [dbo].[Evidencias] CHECK CONSTRAINT [FK_Evidencias_Unidades]
GO
ALTER TABLE [dbo].[Profesores]  WITH CHECK ADD  CONSTRAINT [FK_Profesores_Profesores] FOREIGN KEY([userName])
REFERENCES [dbo].[Usuarios] ([userName])
GO
ALTER TABLE [dbo].[Profesores] CHECK CONSTRAINT [FK_Profesores_Profesores]
GO
ALTER TABLE [dbo].[Recursos]  WITH CHECK ADD  CONSTRAINT [FK_Recursos_Usuarios] FOREIGN KEY([userName])
REFERENCES [dbo].[Usuarios] ([userName])
GO
ALTER TABLE [dbo].[Recursos] CHECK CONSTRAINT [FK_Recursos_Usuarios]
GO
ALTER TABLE [dbo].[Tema]  WITH CHECK ADD  CONSTRAINT [FK_Tema_Usuarios] FOREIGN KEY([userName])
REFERENCES [dbo].[Usuarios] ([userName])
GO
ALTER TABLE [dbo].[Tema] CHECK CONSTRAINT [FK_Tema_Usuarios]
GO
ALTER TABLE [dbo].[Unidades]  WITH CHECK ADD  CONSTRAINT [FK_Unidades_Tema] FOREIGN KEY([idTema])
REFERENCES [dbo].[Tema] ([idTema])
GO
ALTER TABLE [dbo].[Unidades] CHECK CONSTRAINT [FK_Unidades_Tema]
GO
ALTER TABLE [dbo].[Unidades]  WITH CHECK ADD  CONSTRAINT [FK_Unidades_Usuarios] FOREIGN KEY([userName])
REFERENCES [dbo].[Usuarios] ([userName])
GO
ALTER TABLE [dbo].[Unidades] CHECK CONSTRAINT [FK_Unidades_Usuarios]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_NivelEstudio] FOREIGN KEY([idNivelEstudios])
REFERENCES [dbo].[NivelEstudio] ([idNivelEstudios])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_NivelEstudio]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Rol] FOREIGN KEY([idRol])
REFERENCES [dbo].[Rol] ([idRol])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Rol]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo numérico autoincremental que contiene el id principal del registro de la auditoria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Auditoria', @level2type=N'COLUMN',@level2name=N'idAuditoria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre de la tabla la cual tuvo cambios por parte del usuario.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Auditoria', @level2type=N'COLUMN',@level2name=N'tabla'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción de los cambios realizados por parte del usuario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Auditoria', @level2type=N'COLUMN',@level2name=N'registro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la modificación de la información de la tabla y queda registrada la auditoría.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Auditoria', @level2type=N'COLUMN',@level2name=N'fecha'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria de la tabla Usuarios e identificador del usuario que se está auditando.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Auditoria', @level2type=N'COLUMN',@level2name=N'userName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'abla que registra los eventos del CRUD ocasionados a las tablas de la DB.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Auditoria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo numérico autoincremental que contiene el id principal del registro de la configuración del servidor para el envío de correos electrónicos al usuario.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'configEmail', @level2type=N'COLUMN',@level2name=N'idConfigEmail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre o Alias del dueño del correo electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'configEmail', @level2type=N'COLUMN',@level2name=N'nombre'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dirección del servidor SMTP tilizado para el intercambio de mensajes de correo electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'configEmail', @level2type=N'COLUMN',@level2name=N'servidor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dirección de correo electrónico del sistema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'configEmail', @level2type=N'COLUMN',@level2name=N'usuario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contraseña de seguridad para acceder a la cuenta de correo electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'configEmail', @level2type=N'COLUMN',@level2name=N'clave'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número del puerto utilizado por el servidor SMTP para la transferencia de correos electrónicos.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'configEmail', @level2type=N'COLUMN',@level2name=N'puerto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de Puerto (465) que se utiliza para cifrado SSL, otorgando mayor seguridad del envío de correos pues suele usarse para cifrar comunicaciones en Internet' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'configEmail', @level2type=N'COLUMN',@level2name=N'ssl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla para el registro de la configuración del servidor de envío de correos electrónicos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'configEmail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo numérico autoincremental que contiene el id principal del registro de un profesor.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Profesores', @level2type=N'COLUMN',@level2name=N'idProfesor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del título profesional que tiene el profesor.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Profesores', @level2type=N'COLUMN',@level2name=N'profesion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción de los logros y experiencia profesional del profesor.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Profesores', @level2type=N'COLUMN',@level2name=N'perfilProfesional'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fotografía personal del profesor almacenada de manera decoficada.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Profesores', @level2type=N'COLUMN',@level2name=N'fotografia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria de la tabla Usuarios e identificador qeu permite asociar el usuario con el profesor.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Profesores', @level2type=N'COLUMN',@level2name=N'userName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla donde se almacenan los usuarios registrados con rol profesor en el sistema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Profesores'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo numérico autoincremental que contiene el id principal del registro de un recurso digital [Los recursos digitales pueden ser videos, archivos pdf o imágenes].' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'idRecurso'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Texto con el que se designa y se distingue un recurso digital' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'nombre'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del contenido del recurso digital' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'descripcion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dirección web en donde está alojado el recurso digital con la definición del tema en vídeo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Recurso digital decodificado que contiene la definición del tema en formato pdf' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'archivo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Recurso digital decodificado que contiene la definición del tema en formato imagen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'imagen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo booleano para la validación del estado de un recurso digital.  [0] para Activo y [1] para Inactivo.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'estado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria de la tabla Unidad e identificador que permite asociar el recurso a la Unidad.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'idUnidad'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la creación de un recurso digital.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'fecha'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la modificación de un recurso digital.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'fechaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria de la tabla Usuarios e identificador del usuario que creó el recurso digital.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos', @level2type=N'COLUMN',@level2name=N'userName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla para el registro de los recursos digitales que esten asociadas la una unidad registrada en sistema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recursos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo numérico autoincremental que contiene el id principal del registro de un rol en el sistema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rol', @level2type=N'COLUMN',@level2name=N'idRol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Texto con el que se designa y se distingue un rol.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rol', @level2type=N'COLUMN',@level2name=N'nombreRol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla para el registro de los distintos roles que pueden tener los usuarios en el sistema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo numérico autoincremental que contiene el id principal del registro de un tema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tema', @level2type=N'COLUMN',@level2name=N'idTema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Texto con el que se designa y se distingue un tema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tema', @level2type=N'COLUMN',@level2name=N'nombre'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción de las características del tema propuesto para estudio.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tema', @level2type=N'COLUMN',@level2name=N'descripcion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo booleano para la validación del estado de un tema.  [0] para Activo y [1] para Inactivo.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tema', @level2type=N'COLUMN',@level2name=N'estado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la creación de un tema.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tema', @level2type=N'COLUMN',@level2name=N'fecha'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la moficación de un tema.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tema', @level2type=N'COLUMN',@level2name=N'fechaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria de la tabla Usuarios e identificador del usuario que creó el tema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tema', @level2type=N'COLUMN',@level2name=N'userName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla para el registro de los temas que contiene la plataforma.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo numérico autoincremental que contiene el id principal del registro de la unidad de un tema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades', @level2type=N'COLUMN',@level2name=N'idUnidad'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Texto con el que se designa y se distingue la unidad de un tema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades', @level2type=N'COLUMN',@level2name=N'nombre'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del contenido o características de la unidad de un tema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades', @level2type=N'COLUMN',@level2name=N'descripcion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo booleano para la validación del estado de la unidad de un tema.  [0] para Activo y [1] para Inactivo.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades', @level2type=N'COLUMN',@level2name=N'estado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria de la tabla Tema e identificador que permite asociar la unidad a un tema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades', @level2type=N'COLUMN',@level2name=N'idTema'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la creación de la unidad de un tema en el sistema.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades', @level2type=N'COLUMN',@level2name=N'fecha'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la modificación de la unidad de un tema en el sistema.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades', @level2type=N'COLUMN',@level2name=N'fechaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria de la tabla Usuarios e identificador del usuario que creó la unidad del tema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades', @level2type=N'COLUMN',@level2name=N'userName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla que almacena el registro de las unidades asociadas a los temas.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo numérico autoincremental que contiene el número id del registro de un usuario en el sistema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'idUserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo de texto que contiene el nombre de usuario de una persona en el sistema, hace las veces de llave principal en esta tabla.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'userName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contiene el registro de caracteres que se relacionan al registro de usuario, y que se establece como parámetro de seguridad que confirma la identidad de la persona que accede al sistema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'userPassword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dirección de correo electrónico que se asocia a la persona registrada.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'correoElectronico'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo de tipo texto que contiene el registro del Nombre de la persona registrada en el sistema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'nombre'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo de tipo texto que contiene el registro del Apellido de la persona registrada en el sistema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'apellidos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo booleano para la validación del estado de un usuario en el sistema.  [0] para Activo y [1] para Inactivo.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'estado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria de la tabla Rol e identificador que permite asociar al rol con el usuario del sistema.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'idRol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la creación del usuario en el sistema.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'fecha'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Día, Mes y Año en el que se hace la modificación del usuario en el sistema.  Esta información debe estar en formato (dd/mm/aaaa)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios', @level2type=N'COLUMN',@level2name=N'fechaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla que contiene los datos personales de los usuarios registrados en el sistema, también contiene las credenciales de acceso registradas a la plataforma.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Usuarios'
GO
