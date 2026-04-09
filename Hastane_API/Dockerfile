# 1. AŞAMA: DERLEME (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build 
# Microsoft'un deposundan içinde .NET 9.0 SDK kurulu olan imajı indir ve build adını ver
WORKDIR /app
# Konteyner içinde app dında bir klasör oluşturuğ içine gir

COPY *.csproj ./
# csproj dosyasının içindekileri konteynere bağla
RUN dotnet restore
# dotnet restore komutunu çalıştır

# Geri kalan her şeyi kopyalayıp projeyi publish ediyoruz
COPY . ./
RUN dotnet publish -c Release -o out
# publish ile uygulamayı sunucuya göndermeye hazır hale getiriyoruz
# relase daha hızlı olması için. out ise derlenen her şeyin /app/out klasörüne atılmasını sağlıyor
# .dll dosyaları oluşturuyor.



# 2. AŞAMA: ÇALIŞTIRMA (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
# SDK'dan ürettiğimiz .dll dosyasını çalıştıracak olan imajı indir ve runtime adını ver
WORKDIR /app

# build adlı konteynera gidip /app/out klasöründeki dosyaları kopyalıyor. Yani oluşturulan .dll dosyaları
COPY --from=build /app/out . 

# Uygulamanın hangi dosya ile başlayacağını belirtiyoruz
ENTRYPOINT ["dotnet", "MyApiProject.dll"]
