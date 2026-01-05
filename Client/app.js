const API_URL = 'http://localhost:5269/api/v1'; // Port actualizat

// State
let selectedProductId = null;
const userId = "11111111-1111-1111-1111-111111111111"; // Mock User ID pentru demo

// Funcționalitate meniu mobil
document.addEventListener('DOMContentLoaded', function() {
    const mobileMenuButton = document.getElementById('mobile-menu-button');
    const mobileMenu = document.getElementById('mobile-menu');
    
    if (mobileMenuButton && mobileMenu) {
        mobileMenuButton.addEventListener('click', function() {
            mobileMenu.classList.toggle('hidden');
        });

        // Închide meniul mobil când se face clic în afara lui
        document.addEventListener('click', function(event) {
            if (!mobileMenuButton.contains(event.target) && !mobileMenu.contains(event.target)) {
                mobileMenu.classList.add('hidden');
            }
        });
    }

    loadProducts();
    
    // Event listeners pentru încărcarea dinamică a cabinelor
    const dateInput = document.getElementById('res-date');
    const timeInput = document.getElementById('res-time');
    
    if (dateInput) dateInput.addEventListener('change', loadAvailableCabins);
    if (timeInput) timeInput.addEventListener('change', loadAvailableCabins);
});

async function loadProducts() {
    const list = document.getElementById('product-list');
    list.innerHTML = '<div class="col-span-full flex justify-center items-center py-20"><div class="text-center"><div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-accent mb-4"></div><p class="text-gray-500 text-lg">Se încarcă produsele<span class="loading-dots"></span></p></div></div>';

    try {
        let products = [];
        
        // Încearcă să obții date reale din API
        try {
            const response = await axios.get(`${API_URL}/products`);
            products = response.data;
        } catch(e) { 
            console.log("Backend-ul nu este accesibil, folosind date mock pentru demo UI");
            // SENİN BULDUĞUN TAM URL'LER İLE GÜNCEL FOTOĞRAFLAR
            products = [
                { id: 1, name: 'Rochie Florală de Vară', price: 299.99, imageUrl: 'https://images.unsplash.com/photo-eine-frau-in-einem-kleid-mit-blauem-blumenprint-FGOpeBaiklg?w=400&h=600&fit=crop', color: 'Roz Pudră', size: 'M', stock: 5 },
                { id: 2, name: 'Rochie de Seară Neagră', price: 599.99, imageUrl: 'https://images.unsplash.com/photo-a-woman-posing-for-a-picture-in-a-black-dress-zj3GjVWzCIE?w=400&h=600&fit=crop', color: 'Negru', size: 'S', stock: 3 },
                { id: 3, name: 'Rochie Cocktail Albastră', price: 449.99, imageUrl: 'https://images.unsplash.com/photo-eine-frau-in-einem-kleid-mit-blauem-blumenprint-FGOpeBaiklg?w=400&h=600&fit=crop', color: 'Albastru Royal', size: 'L', stock: 4 },
                { id: 4, name: 'Costum Business Feminin', price: 799.99, imageUrl: 'https://images.unsplash.com/photo-happy-cheerful-business-lady-in-blue-formal-clothes-blJ3pjjgwZ0?w=400&h=600&fit=crop', color: 'Gri Antracit', size: 'M', stock: 6 },
                { id: 5, name: 'Blazer Premium cu Pantaloni', price: 699.99, imageUrl: 'https://images.unsplash.com/photo-a-woman-in-a-blue-suit-is-posing-for-a-picture-Nb3ptC1E37Q?w=400&h=600&fit=crop', color: 'Bleumarin', size: 'S', stock: 4 },
                { id: 6, name: 'Jachetă Denim Vintage', price: 189.99, imageUrl: 'https://images.unsplash.com/photo-woman-in-blue-denim-jacket-0SOwslg0-YY?w=400&h=600&fit=crop', color: 'Albastru Deschis', size: 'M', stock: 8 },
                { id: 7, name: 'Pulover Cashmere', price: 349.99, imageUrl: 'https://images.unsplash.com/photo-woman-in-brown-knit-sweater-and-white-pants-hPnUtt4LWJo?w=400&h=600&fit=crop', color: 'Bej', size: 'L', stock: 5 },
                { id: 8, name: 'Pantaloni Palazzo', price: 229.99, imageUrl: 'https://images.unsplash.com/photo-a-woman-in-a-woman-in-black-top-and-black-pants-u4vvfCC5mQ0?w=400&h=600&fit=crop', color: 'Verde Oliv', size: 'M', stock: 6 },
                { id: 9, name: 'Rochie Midi Elegantă', price: 399.99, imageUrl: 'https://images.unsplash.com/photo-eine-frau-in-einem-kleid-mit-blauem-blumenprint-FGOpeBaiklg?w=400&h=600&fit=crop', color: 'Bordo', size: 'S', stock: 3 },
                { id: 10, name: 'Costum Trei Piese', price: 999.99, imageUrl: 'https://images.unsplash.com/photo-happy-cheerful-business-lady-in-blue-formal-clothes-blJ3pjjgwZ0?w=400&h=600&fit=crop', color: 'Negru', size: 'L', stock: 2 },
                { id: 11, name: 'Rochie Boho Chic', price: 279.99, imageUrl: 'https://images.unsplash.com/photo-woman-sits-outdoors-in-a-floral-dress-2j4AtTew4i8?w=400&h=600&fit=crop', color: 'Crem', size: 'M', stock: 4 },
                { id: 12, name: 'Jachetă Piele Premium', price: 899.99, imageUrl: 'https://images.unsplash.com/photo-woman-in-black-jacket-beside-woman-in-blue-denim-jacket-WdcoO8j8okk?w=400&h=600&fit=crop', color: 'Maro Cognac', size: 'M', stock: 3 },
                { id: 13, name: 'Rochie Wrap Elegantă', price: 329.99, imageUrl: 'https://images.unsplash.com/photo-eine-frau-in-einem-kleid-mit-blauem-blumenprint-FGOpeBaiklg?w=400&h=600&fit=crop', color: 'Verde Smarald', size: 'S', stock: 5 },
                { id: 14, name: 'Costum Casual Linen', price: 459.99, imageUrl: 'https://images.unsplash.com/photo-a-woman-in-a-blue-suit-is-posing-for-a-picture-Nb3ptC1E37Q?w=400&h=600&fit=crop', color: 'Bej Natural', size: 'M', stock: 7 },
                { id: 15, name: 'Rochie Maxi Florală', price: 389.99, imageUrl: 'https://images.unsplash.com/photo-eine-frau-in-einem-kleid-mit-blauem-blumenprint-FGOpeBaiklg?w=400&h=600&fit=crop', color: 'Multicolor', size: 'L', stock: 4 },
                { id: 16, name: 'Blazer Structured', price: 549.99, imageUrl: 'https://images.unsplash.com/photo-a-woman-in-a-blue-suit-is-posing-for-a-picture-Nb3ptC1E37Q?w=400&h=600&fit=crop', color: 'Roz Pudră', size: 'S', stock: 6 }
            ];
        }

        if (products.length === 0) {
            list.innerHTML = '<div class="col-span-full text-center"><p class="text-gray-500">Nu sunt produse disponibile.</p></div>';
            return;
        }

        list.innerHTML = products.map(p => `
            <div class="product-card bg-white rounded-xl shadow-lg overflow-hidden hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-2">
                <div class="relative aspect-w-3 aspect-h-4">
                    <img src="${p.imageUrl || 'https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=400&h=600&fit=crop'}" 
                         alt="${p.name}" 
                         class="w-full h-80 object-cover"
                         loading="lazy"
                         onerror="this.src='https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=400&h=600&fit=crop'">
                    <div class="absolute top-4 right-4">
                        <span class="bg-white bg-opacity-90 text-gray-800 px-3 py-1 rounded-full text-sm font-medium">
                            ${p.stock || 'În stoc'}
                        </span>
                    </div>
                    ${p.color ? `<div class="absolute top-4 left-4">
                        <span class="bg-black bg-opacity-70 text-white px-2 py-1 rounded text-xs">
                            ${p.color}
                        </span>
                    </div>` : ''}
                </div>
                <div class="p-6">
                    <div class="flex justify-between items-start mb-3">
                        <h3 class="text-xl font-bold text-gray-900 leading-tight">${p.name}</h3>
                        ${p.size ? `<span class="bg-gray-100 text-gray-700 px-2 py-1 rounded text-sm font-medium">${p.size}</span>` : ''}
                    </div>
                    <div class="flex items-center justify-between mb-4">
                        <p class="text-3xl font-bold text-accent">${p.price} RON</p>
                        <div class="flex items-center text-yellow-400">
                            <svg class="w-4 h-4 fill-current" viewBox="0 0 20 20">
                                <path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/>
                            </svg>
                            <svg class="w-4 h-4 fill-current" viewBox="0 0 20 20">
                                <path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/>
                            </svg>
                            <svg class="w-4 h-4 fill-current" viewBox="0 0 20 20">
                                <path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/>
                            </svg>
                            <svg class="w-4 h-4 fill-current" viewBox="0 0 20 20">
                                <path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/>
                            </svg>
                            <svg class="w-4 h-4 fill-current" viewBox="0 0 20 20">
                                <path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/>
                            </svg>
                            <span class="ml-1 text-gray-600 text-sm">(4.8)</span>
                        </div>
                    </div>
                    <button onclick="openReservationModal(${p.id}, '${p.name}')" 
                            class="w-full bg-gradient-to-r from-accent to-indigo-600 text-white py-4 px-6 rounded-xl hover:from-indigo-600 hover:to-accent transition-all duration-300 font-semibold text-lg shadow-lg hover:shadow-xl transform hover:scale-105">
                        <span class="flex items-center justify-center">
                            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3a2 2 0 012-2h4a2 2 0 012 2v4m-6 0V6a2 2 0 012-2h4a2 2 0 012 2v1m-6 0h8m-8 0H6a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V9a2 2 0 00-2-2h-2"/>
                            </svg>
                            Rezervare Cabină
                        </span>
                    </button>
                </div>
            </div>
        `).join('');

    } catch (error) {
        console.error('Eroare la încărcarea produselor:', error);
        list.innerHTML = '<div class="col-span-full text-center"><p class="text-red-500">Eșec la încărcarea produselor. Vă rugăm să încercați din nou.</p></div>';
    }
}

async function openReservationModal(productId, productName) {
    selectedProductId = productId;
    document.getElementById('modal-product-name').innerText = `Rezervare pentru: ${productName}`;
    document.getElementById('reservation-modal').classList.remove('hidden');
    
    // Setează data implicită la astăzi
    const today = new Date();
    document.getElementById('res-date').valueAsDate = today;
    
    // Încarcă cabinele disponibile
    await loadAvailableCabins();
}

async function loadAvailableCabins() {
    const cabinSelect = document.getElementById('res-cabin');
    const date = document.getElementById('res-date').value;
    const time = document.getElementById('res-time').value;
    
    if (!date || !time || !selectedProductId) {
        cabinSelect.innerHTML = '<option value="">Selectează mai întâi data și ora</option>';
        return;
    }

    try {
        cabinSelect.innerHTML = '<option value="">Se încarcă...</option>';
        
        const response = await axios.get(`${API_URL}/reservations/available-cabins`, {
            params: {
                productId: selectedProductId,
                date: date,
                hour: time
            }
        });
        
        const cabins = response.data;
        
        if (cabins.length === 0) {
            cabinSelect.innerHTML = '<option value="">Nu sunt cabine disponibile la această oră</option>';
            return;
        }
        
        cabinSelect.innerHTML = '<option value="">Selectează cabina</option>' + 
            cabins.filter(c => c.isAvailable)
                  .map(c => `<option value="${c.id}">Cabina ${c.cabinNumber}</option>`)
                  .join('');
                  
    } catch (error) {
        console.error('Eroare la încărcarea cabinelor:', error);
        // Date mock de rezervă
        cabinSelect.innerHTML = `
            <option value="">Selectează cabina</option>
            <option value="1">Cabina 1</option>
            <option value="2">Cabina 2</option>
            <option value="3">Cabina 3</option>
        `;
    }
}

function closeModal() {
    document.getElementById('reservation-modal').classList.add('hidden');
}

async function submitReservation() {
    const date = document.getElementById('res-date').value;
    const time = document.getElementById('res-time').value;
    const cabinId = document.getElementById('res-cabin').value;

    if(!date || !time || !cabinId) {
        alert("Vă rugăm să completați toate câmpurile");
        return;
    }

    const submitButton = document.querySelector('#reservation-modal button[onclick="submitReservation()"]');
    const originalText = submitButton.textContent;
    submitButton.textContent = 'Se face rezervarea...';
    submitButton.disabled = true;

    try {
        const payload = {
            userId: userId,
            productId: selectedProductId,
            cabinId: parseInt(cabinId),
            date: date,
            hour: time
        };

        const response = await axios.post(`${API_URL}/reservations`, payload);
        
        closeModal();
        showSuccess(response.data.accessCode);

    } catch (error) {
        console.error('Eroare rezervare:', error);
        const errorMessage = error.response?.data?.message || 
                           error.response?.data?.error || 
                           error.message || 
                           'Rezervarea nu a putut fi făcută';
        alert("Eroare rezervare: " + errorMessage);
    } finally {
        submitButton.textContent = originalText;
        submitButton.disabled = false;
    }
}

function showSuccess(code) {
    document.getElementById('success-code').innerText = code;
    document.getElementById('success-modal').classList.remove('hidden');
}

function closeSuccessModal() {
    document.getElementById('success-modal').classList.add('hidden');
}