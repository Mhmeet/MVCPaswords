document.addEventListener("DOMContentLoaded", function () { //sayfa yüklendiğinde
	var lazyloadImages = document.querySelectorAll("img.lazy"); //sınıfı lazy olan img etiketlerini seç
	var lazyloadThrottleTimeout;

	function lazyload() {
		if (lazyloadThrottleTimeout) { //setTimeout calisti onceden calisti ise
			clearTimeout(lazyloadThrottleTimeout); //sil, temizle
		}

		lazyloadThrottleTimeout = setTimeout(function () { //20 mili saniye sonra calistir
			var scrollTop = window.pageYOffset; //sayfanın yukarıdan aşağı konumu
			lazyloadImages.forEach(function (img) { //sınıfı lazy olan img etiketleri üzerinde dön
				if (img.offsetTop < (window.innerHeight + scrollTop)) { //sayfa aşağı yada yukarı inerken bu resimlere denk gelinirse
					img.src = img.dataset.src; //img etiketinde data-src özelliğindeki resim yolunu src özelliğine aktar
					img.classList.remove('lazy'); //img etiketindeki class lardan lazy değerini sil, lazy class ını sil
				}
			});
			if (lazyloadImages.length == 0) {  //sayfada lazy class lı img etiketi bulunmuyorsa
				document.removeEventListener("scroll", lazyload); //sayfanın scroll hareketini izleme olayını kaldır
				window.removeEventListener("resize", lazyload); //sayfanın yeniden boyutlandırma hareketini izleme olayını kaldır
				window.removeEventListener("orientationChange", lazyload);
				//mobil cihazlarda ekranın dik yada yan tutulması ile ekranın genişliğinin yeniden boyutlandırılması hareketini izleme olayını kaldır
			}
		}, 20);
	}
})