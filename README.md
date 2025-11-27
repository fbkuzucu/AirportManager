Merhabalar,

Öncelikle yapmış olduğum demoyu incelediğiniz için teşekkür ederim. Bu süreçte olabildiğince örnek oyuna benzer kalmaya gayret ettim ancak bazı farklılıklar var.
Bagaj animasyonlarını koyamadım ancak animasyon koyabileceğimiz altyapıyı geliştirmeye çalıştım ve ayrıca arkadaki state döngüsü devam ediyor ve görebilmeniz için UI'a ekledim.
Onun dışında yapılan işlerde (Bagaj kontrolü, Check In vb.) oyuncuyu alanda tutan herhangi bir kilit yok ama diğer state'e girmesi için alanda işlem süresi kadar beklemesi gerek. Bu işlem süresi ortalama her yolcu için 1-2 saniye olacak şekilde geliştirildi. Bunu UI'da göremeyebilirsiniz ancak loglarda her yolcuya göre işlem olduğunu görebilirsiniz.
İlgili kodu incelemek isterseniz de WorkStation() sınıfına bakabilirsiniz.
Ben temsili olarak yeni bölgeler açmak için kullanılan bölgeler için cost'ları koydum ancak değiştirmek isterseniz UnlockEvent() sınıfına ve EventUnlocker prefab'ına bakabilirsiniz.

Teşekkür eder, iyi günler dilerim.
