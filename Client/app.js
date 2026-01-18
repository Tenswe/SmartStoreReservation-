const API_URL = 'http://localhost:5270/api/v1'; // Port güncellendi

// State
let selectedProductId = null;

// Filter Products - Global scope'ta tanımla
let allProducts = [];
let currentFilter = 'all';

// Global fonksiyonları window objesine ekle
window.filterProducts = filterProducts;
window.openReservationModal = openReservationModal;
window.rateProduct = rateProduct;
window.openMyReservationsModal = openMyReservationsModal;
window.closeMyReservationsModal = closeMyReservationsModal;
window.cancelReservation = cancelReservation;
window.openLoginModal = openLoginModal;
window.closeLoginModal = closeLoginModal;
window.handleLogin = handleLogin;
window.openRegisterModal = openRegisterModal;
window.closeRegisterModal = closeRegisterModal;
window.handleRegister = handleRegister;
window.toggleUserMenu = toggleUserMenu;
window.closeUserMenu = closeUserMenu;
window.logout = logout;
window.closeModal = closeModal;
window.submitReservation = submitReservation;
window.closeSuccessModal = closeSuccessModal;

// Initialize everything when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM loaded, initializing...');
    
    // Check if user is logged in and update UI
    updateUIForLoggedInUser();
    
    // Mobile menu functionality
    const mobileMenuButton = document.getElementById('mobile-menu-button');
    const mobileMenu = document.getElementById('mobile-menu');
    
    if (mobileMenuButton && mobileMenu) {
        mobileMenuButton.addEventListener('click', function() {
            mobileMenu.classList.toggle('hidden');
        });

        // Close mobile menu when clicking outside
        document.addEventListener('click', function(event) {
            if (!mobileMenuButton.contains(event.target) && !mobileMenu.contains(event.target)) {
                mobileMenu.classList.add('hidden');
            }
        });
    }

    // Close user menu when clicking outside
    document.addEventListener('click', function(event) {
        const menu = document.getElementById('user-menu');
        if (menu && !menu.classList.contains('hidden')) {
            if (!event.target.closest('#user-menu') && !event.target.closest('button[onclick*="toggleUserMenu"]')) {
                closeUserMenu();
            }
        }
    });

    // Event listeners for cabin loading
    const dateInput = document.getElementById('res-date');
    const timeInput = document.getElementById('res-time');
    
    if (dateInput) dateInput.addEventListener('change', loadAvailableCabins);
    if (timeInput) timeInput.addEventListener('change', loadAvailableCabins);

    // Load products
    loadProducts();
});

async function loadProducts() {
    const list = document.getElementById('product-list');
    list.innerHTML = '<div class="col-span-full flex justify-center items-center py-20"><div class="text-center"><div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-accent mb-4"></div><p class="text-gray-500 text-lg">Se încarcă produsele<span class="loading-dots"></span></p></div></div>';

    try {
        const response = await axios.get(`${API_URL}/products`);
        allProducts = response.data;
        console.log('Products loaded:', allProducts.length);
        renderProducts(allProducts);
    } catch (error) {
        console.error('Eroare la încărcarea produselor:', error);
        list.innerHTML = '<div class="col-span-full text-center"><p class="text-red-500">Eșec la încărcarea produselor. Vă rugăm să încercați din nou.</p></div>';
    }
}

function filterProducts(category) {
    console.log('=== FILTER CALLED ===');
    console.log('Category:', category);
    console.log('All products:', allProducts.length);
    currentFilter = category;

    // Update button styles
    document.querySelectorAll('[id^="filter-"]').forEach(btn => {
        btn.classList.remove('bg-gray-900', 'text-white');
        btn.classList.add('bg-gray-200', 'text-gray-700');
    });
    
    const activeBtn = document.getElementById('filter-' + category);
    if (activeBtn) {
        activeBtn.classList.remove('bg-gray-200', 'text-gray-700');
        activeBtn.classList.add('bg-gray-900', 'text-white');
        console.log('Button activated:', 'filter-' + category);
    } else {
        console.error('Button not found:', 'filter-' + category);
    }

    // Filter products
    if (category === 'all') {
        console.log('Showing all products:', allProducts.length);
        renderProducts(allProducts);
    } else {
        const filtered = allProducts.filter(p => {
            const name = p.name.toLowerCase();
            if (category === 'rochii') {
                return name.includes('rochie');
            } else if (category === 'costume') {
                return name.includes('costum') || name.includes('blazer');
            }
            return false;
        });
        console.log(`Filtered ${category}:`, filtered.length, 'products');
        renderProducts(filtered);
    }
}

function renderProducts(products) {
    const list = document.getElementById('product-list');
    
    if (products.length === 0) {
        list.innerHTML = '<div class="col-span-full text-center py-12"><p class="text-gray-500 text-lg">Nu sunt produse în această categorie</p></div>';
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
                    <div class="flex items-center text-yellow-400" onclick="rateProduct(${p.id}, event)">
                        ${generateStars(p.rating || 4.8)}
                        <span class="ml-1 text-gray-600 text-sm">(${p.rating || 4.8})</span>
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
}

function generateStars(rating) {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    let stars = '';
    
    for (let i = 0; i < 5; i++) {
        if (i < fullStars) {
            stars += '<svg class="w-4 h-4 fill-current cursor-pointer hover:scale-110 transition" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>';
        } else if (i === fullStars && hasHalfStar) {
            stars += '<svg class="w-4 h-4 fill-current cursor-pointer hover:scale-110 transition" viewBox="0 0 20 20"><defs><linearGradient id="half"><stop offset="50%" stop-color="currentColor"/><stop offset="50%" stop-color="#D1D5DB"/></linearGradient></defs><path fill="url(#half)" d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>';
        } else {
            stars += '<svg class="w-4 h-4 text-gray-300 fill-current cursor-pointer hover:scale-110 transition" viewBox="0 0 20 20"><path d="M10 15l-5.878 3.09 1.123-6.545L.489 6.91l6.572-.955L10 0l2.939 5.955 6.572.955-4.756 4.635 1.123 6.545z"/></svg>';
        }
    }
    return stars;
}

function rateProduct(productId, event) {
    event.stopPropagation();
    
    const stars = event.currentTarget.querySelectorAll('svg');
    const rect = event.currentTarget.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const starWidth = rect.width / 5;
    const rating = Math.ceil(x / starWidth);
    
    if (confirm(`Doriți să evaluați acest produs cu ${rating} stele?`)) {
        console.log(`Product ${productId} rated with ${rating} stars`);
        alert(`Mulțumim! Ați evaluat produsul cu ${rating} stele.`);
        
        // Update UI
        const product = allProducts.find(p => p.id === productId);
        if (product) {
            product.rating = rating;
            renderProducts(currentFilter === 'all' ? allProducts : allProducts.filter(p => {
                const name = p.name.toLowerCase();
                if (currentFilter === 'rochii') return name.includes('rochie');
                if (currentFilter === 'costume') return name.includes('costum') || name.includes('blazer');
                return false;
            }));
        }
    }
}

async function openReservationModal(productId, productName) {
    if (!isUserLoggedIn()) {
        alert('Vă rugăm să vă autentificați pentru a face o rezervare.');
        openLoginModal();
        return;
    }

    selectedProductId = productId;
    document.getElementById('modal-product-name').innerText = `Rezervare pentru: ${productName}`;
    document.getElementById('reservation-modal').classList.remove('hidden');
    
    // Setează data implicită la astăzi
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('res-date').value = today;
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
    const userId = getCurrentUserId();
    if (!userId) return;

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


// My Reservations Modal Functions
function openMyReservationsModal() {
    console.log('=== OPEN MY RESERVATIONS ===');
    console.log('Is logged in:', isUserLoggedIn());
    console.log('User ID:', localStorage.getItem('userId'));
    
    if (!isUserLoggedIn()) {
        alert('Vă rugăm să vă autentificați pentru a vedea rezervările.');
        openLoginModal();
        return;
    }
    
    document.getElementById('my-reservations-modal').classList.remove('hidden');
    loadMyReservations();
}

function closeMyReservationsModal() {
    document.getElementById('my-reservations-modal').classList.add('hidden');
}

async function loadMyReservations() {
    console.log('=== LOAD MY RESERVATIONS ===');
    const userId = getCurrentUserId();
    console.log('Got user ID:', userId);
    if (!userId) return;

    const list = document.getElementById('reservations-list');
    list.innerHTML = '<div class="text-center py-8"><div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-accent mb-4"></div><p class="text-gray-500">Se încarcă rezervările...</p></div>';

    try {
        const url = `${API_URL}/reservations/user/${userId}`;
        console.log('Fetching from:', url);
        const response = await axios.get(url);
        const reservations = response.data;
        console.log('Reservations received:', reservations.length);

        if (reservations.length === 0) {
            list.innerHTML = `
                <div class="text-center py-12">
                    <svg class="w-16 h-16 mx-auto text-gray-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"></path>
                    </svg>
                    <p class="text-gray-500 text-lg">Nu ai rezervări încă</p>
                    <p class="text-gray-400 text-sm mt-2">Începe să explorezi colecția noastră!</p>
                </div>
            `;
            return;
        }

        list.innerHTML = reservations.map(r => {
            const statusColors = {
                'Pending': 'bg-yellow-100 text-yellow-800',
                'Confirmată': 'bg-green-100 text-green-800',
                'Completed': 'bg-blue-100 text-blue-800',
                'Anulată': 'bg-red-100 text-red-800'
            };
            const statusColor = statusColors[r.status] || 'bg-gray-100 text-gray-800';

            return `
                <div class="bg-gray-50 rounded-xl p-6 mb-4 hover:shadow-lg transition-shadow">
                    <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                        <div class="flex-1">
                            <div class="flex items-center gap-3 mb-2">
                                <h3 class="text-lg font-bold text-gray-900">${r.productName || 'Produs'}</h3>
                                <span class="px-3 py-1 rounded-full text-xs font-medium ${statusColor}">
                                    ${r.status}
                                </span>
                            </div>
                            <div class="space-y-1 text-sm text-gray-600">
                                <p><strong>Data:</strong> ${new Date(r.date).toLocaleDateString('ro-RO')}</p>
                                <p><strong>Ora:</strong> ${r.hour}</p>
                                <p><strong>Cabina:</strong> ${r.cabinNumber || r.cabinId}</p>
                                ${r.accessCode ? `<p><strong>Cod acces:</strong> <span class="font-mono bg-white px-2 py-1 rounded">${r.accessCode}</span></p>` : ''}
                            </div>
                        </div>
                        ${r.status === 'Pending' || r.status === 'Confirmată' ? `
                        <div class="flex gap-2">
                            <button onclick="cancelReservation(${r.id})" 
                                    class="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition text-sm font-medium">
                                Anulează
                            </button>
                        </div>
                        ` : ''}
                    </div>
                </div>
            `;
        }).join('');

    } catch (error) {
        console.error('Eroare la încărcarea rezervărilor:', error);
        list.innerHTML = `
            <div class="text-center py-12">
                <svg class="w-16 h-16 mx-auto text-red-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                </svg>
                <p class="text-red-500 text-lg">Eroare la încărcarea rezervărilor</p>
                <p class="text-gray-400 text-sm mt-2">Vă rugăm să încercați din nou</p>
            </div>
        `;
    }
}

async function cancelReservation(reservationId) {
    if (!confirm('Sigur doriți să anulați această rezervare?')) {
        return;
    }

    try {
        await axios.delete(`${API_URL}/reservations/${reservationId}`);
        alert('Rezervarea a fost anulată cu succes!');
        loadMyReservations(); // Reload list
    } catch (error) {
        console.error('Eroare la anularea rezervării:', error);
        alert('Eroare la anularea rezervării. Vă rugăm să încercați din nou.');
    }
}

// Login Modal Functions
function openLoginModal() {
    document.getElementById('login-modal').classList.remove('hidden');
}

function closeLoginModal() {
    document.getElementById('login-modal').classList.add('hidden');
}

async function handleLogin(event) {
    event.preventDefault();
    
    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;

    try {
        const response = await axios.post(`${API_URL}/auth/login`, {
            email: email,
            password: password
        });

        const user = response.data;
        alert(`Bine ai venit, ${user.name}!`);
        
        // Salvează userId în localStorage
        localStorage.setItem('userId', user.userId);
        localStorage.setItem('userName', user.name);
        localStorage.setItem('userEmail', user.email);
        
        closeLoginModal();
        updateUIForLoggedInUser();
        
    } catch (error) {
        console.error('Eroare la autentificare:', error);
        const message = error.response?.data?.message || 'Email sau parolă incorectă';
        alert('Eroare: ' + message);
    }
}

// Register Modal Functions
function openRegisterModal() {
    document.getElementById('register-modal').classList.remove('hidden');
}

function closeRegisterModal() {
    document.getElementById('register-modal').classList.add('hidden');
}

async function handleRegister(event) {
    event.preventDefault();
    
    const name = document.getElementById('register-name').value;
    const email = document.getElementById('register-email').value;
    const password = document.getElementById('register-password').value;

    try {
        const response = await axios.post(`${API_URL}/auth/register`, {
            name: name,
            email: email,
            password: password
        });

        const user = response.data;
        alert(`Înregistrare reușită! Bine ai venit, ${user.name}!`);
        
        // Salvează userId în localStorage
        localStorage.setItem('userId', user.userId);
        localStorage.setItem('userName', user.name);
        localStorage.setItem('userEmail', user.email);
        
        closeRegisterModal();
        updateUIForLoggedInUser();
        
    } catch (error) {
        console.error('Eroare la înregistrare:', error);
        const message = error.response?.data?.message || 'Eroare la înregistrare';
        alert('Eroare: ' + message);
    }
}

// User Management Functions
function getCurrentUserId() {
    const userId = localStorage.getItem('userId');
    if (!userId) {
        alert('Vă rugăm să vă autentificați pentru a continua.');
        openLoginModal();
        return null;
    }
    return userId;
}

function isUserLoggedIn() {
    return localStorage.getItem('userId') !== null;
}

function updateUIForLoggedInUser() {
    const userName = localStorage.getItem('userName');
    if (userName) {
        const firstName = userName.split(' ')[0];
        
        // Desktop menu
        const loginBtn = document.getElementById('login-btn');
        if (loginBtn) {
            loginBtn.innerHTML = `
                <div class="relative inline-block">
                    <button onclick="toggleUserMenu(event)" class="flex items-center space-x-2 text-accent font-semibold hover:text-indigo-700">
                        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
                        </svg>
                        <span>${firstName}</span>
                        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
                        </svg>
                    </button>
                    <div id="user-menu" class="hidden absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-xl border border-gray-200 z-50">
                        <div class="py-2">
                            <div class="px-4 py-2 border-b border-gray-200">
                                <p class="text-sm font-semibold text-gray-900">${userName}</p>
                                <p class="text-xs text-gray-500">${localStorage.getItem('userEmail')}</p>
                            </div>
                            <button onclick="openMyReservationsModal(); closeUserMenu();" class="w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 flex items-center">
                                <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"></path>
                                </svg>
                                Rezervările Mele
                            </button>
                            <button onclick="logout()" class="w-full text-left px-4 py-2 text-sm text-red-600 hover:bg-red-50 flex items-center border-t border-gray-200">
                                <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path>
                                </svg>
                                Deconectare
                            </button>
                        </div>
                    </div>
                </div>
            `;
        }
        
        // Mobile menu
        const loginBtnMobile = document.getElementById('login-btn-mobile');
        if (loginBtnMobile) {
            loginBtnMobile.innerHTML = `
                <div class="border-t border-gray-200 pt-2">
                    <div class="px-3 py-2 bg-gray-50 rounded-lg mb-2">
                        <p class="text-sm font-semibold text-gray-900">${userName}</p>
                        <p class="text-xs text-gray-500">${localStorage.getItem('userEmail')}</p>
                    </div>
                    <button onclick="openMyReservationsModal(); document.getElementById('mobile-menu').classList.add('hidden');" class="w-full text-left px-3 py-2 text-gray-700 hover:text-accent font-medium flex items-center">
                        <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"></path>
                        </svg>
                        Rezervările Mele
                    </button>
                    <button onclick="logout()" class="w-full text-left px-3 py-2 text-red-600 hover:bg-red-50 font-medium flex items-center rounded-lg">
                        <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"></path>
                        </svg>
                        Deconectare
                    </button>
                </div>
            `;
        }
    }
}

function toggleUserMenu(event) {
    event.stopPropagation();
    const menu = document.getElementById('user-menu');
    menu.classList.toggle('hidden');
}

function closeUserMenu() {
    const menu = document.getElementById('user-menu');
    if (menu) {
        menu.classList.add('hidden');
    }
}

function logout() {
    if (confirm('Sigur doriți să vă deconectați?')) {
        localStorage.removeItem('userId');
        localStorage.removeItem('userName');
        localStorage.removeItem('userEmail');
        
        alert('V-ați deconectat cu succes!');
        location.reload();
    }
}
