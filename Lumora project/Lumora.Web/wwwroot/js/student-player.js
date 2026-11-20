/**
 * ============================================================================
 * LUMORA — Learn Beyond Limits | Enterprise Design System
 * File: student-player.js
 * Purpose: Video.js player initialization, lecture playlist navigation,
 *          timestamped notes, bookmarks, and quiz assessment grading.
 * Compatible with: ASP.NET Core MVC (Lumora.Web / wwwroot/js)
 * ============================================================================
 */

(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        // 1. Initialize Video.js Player if container exists
        const videoElement = document.getElementById('lumoraVideoPlayer');
        let player = null;

        if (videoElement && typeof videojs !== 'undefined') {
            player = videojs('lumoraVideoPlayer', {
                controls: true,
                autoplay: false,
                preload: 'auto',
                fluid: true,
                playbackRates: [0.75, 1, 1.25, 1.5, 2]
            });

            player.on('timeupdate', function () {
                const currentTime = player.currentTime();
                const duration = player.duration() || 1;
                const percent = Math.floor((currentTime / duration) * 100);

                const currentProgressBar = document.getElementById('lessonLiveProgressBar');
                if (currentProgressBar) {
                    currentProgressBar.style.width = percent + '%';
                }
            });
        }

        // 2. Curriculum Lesson Switching
        const lessonItems = document.querySelectorAll('.curriculum-lesson-item');
        const playerLessonTitle = document.getElementById('currentLessonTitle');

        lessonItems.forEach(item => {
            item.addEventListener('click', function () {
                lessonItems.forEach(l => l.classList.remove('active'));
                this.classList.add('active');

                const title = this.getAttribute('data-title');
                const duration = this.getAttribute('data-duration');

                if (playerLessonTitle && title) {
                    playerLessonTitle.textContent = title;
                }

                if (player) {
                    player.currentTime(0);
                    player.play();
                }

                if (typeof LumoraToast === 'function') {
                    LumoraToast('Loaded: ' + title);
                }
            });
        });

        // 3. Timestamped Note Taker
        const noteForm = document.getElementById('noteForm');
        const noteInput = document.getElementById('noteInput');
        const notesList = document.getElementById('notesList');

        if (noteForm && noteInput && notesList) {
            noteForm.addEventListener('submit', function (e) {
                e.preventDefault();
                const text = noteInput.value.trim();
                if (!text) return;

                let timeFormatted = '04:25';
                if (player) {
                    const totalSec = Math.floor(player.currentTime());
                    const mins = Math.floor(totalSec / 60);
                    const secs = totalSec % 60;
                    timeFormatted = `${mins < 10 ? '0' : ''}${mins}:${secs < 10 ? '0' : ''}${secs}`;
                }

                const noteCard = document.createElement('div');
                noteCard.className = 'p-3 bg-white rounded-3 border-lumora-subtle mb-2 shadow-subtle';
                noteCard.innerHTML = `
          <div class="d-flex justify-content-between align-items-center mb-1">
            <span class="badge badge-gold cursor-pointer" onclick="jumpToTime('${timeFormatted}')">
              <i class="bi bi-play-circle me-1"></i>${timeFormatted}
            </span>
            <button class="btn btn-link btn-sm text-muted p-0" onclick="this.closest('.p-3').remove()"><i class="bi bi-trash3"></i></button>
          </div>
          <p class="small text-lumora-charcoal mb-0">${text}</p>
        `;

                notesList.prepend(noteCard);
                noteInput.value = '';
                if (typeof LumoraToast === 'function') {
                    LumoraToast('Note recorded at ' + timeFormatted);
                }
            });
        }

        // 4. Bookmark Timestamp Feature
        const addBookmarkBtn = document.getElementById('addBookmarkBtn');
        const bookmarksList = document.getElementById('bookmarksList');

        if (addBookmarkBtn && bookmarksList) {
            addBookmarkBtn.addEventListener('click', function () {
                let timeFormatted = '08:40';
                if (player) {
                    const totalSec = Math.floor(player.currentTime());
                    const mins = Math.floor(totalSec / 60);
                    const secs = totalSec % 60;
                    timeFormatted = `${mins < 10 ? '0' : ''}${mins}:${secs < 10 ? '0' : ''}${secs}`;
                }

                const bookmarkItem = document.createElement('li');
                bookmarkItem.className = 'list-group-item d-flex justify-content-between align-items-center small py-2';
                bookmarkItem.innerHTML = `
          <div class="d-flex align-items-center gap-2">
            <i class="bi bi-bookmark-fill text-lumora-champagne-dark"></i>
            <span>Architecture Key Concept Marker</span>
          </div>
          <span class="badge badge-gold cursor-pointer" onclick="jumpToTime('${timeFormatted}')">${timeFormatted}</span>
        `;
                bookmarksList.prepend(bookmarkItem);

                if (typeof LumoraToast === 'function') {
                    LumoraToast('Bookmark created at ' + timeFormatted);
                }
            });
        }

        // Helper to jump player time
        window.jumpToTime = function (timeStr) {
            if (!player) return;
            const parts = timeStr.split(':');
            const seconds = parseInt(parts[0], 10) * 60 + parseInt(parts[1], 10);
            player.currentTime(seconds);
            player.play();
        };

        // 5. Interactive Section Quiz Assessment Logic
        const quizForm = document.getElementById('assessmentQuizForm');
        const quizResultCard = document.getElementById('quizResultCard');

        if (quizForm && quizResultCard) {
            quizForm.addEventListener('submit', function (e) {
                e.preventDefault();
                // Correct answers simulation
                const answers = {
                    q1: 'b', // PACELC
                    q2: 'c', // Outbox Pattern
                    q3: 'a'  // Idempotency Key
                };

                let score = 0;
                let total = 3;

                const q1Val = (quizForm.querySelector('input[name="q1"]:checked') || {}).value;
                const q2Val = (quizForm.querySelector('input[name="q2"]:checked') || {}).value;
                const q3Val = (quizForm.querySelector('input[name="q3"]:checked') || {}).value;

                if (q1Val === answers.q1) score++;
                if (q2Val === answers.q2) score++;
                if (q3Val === answers.q3) score++;

                const percent = Math.round((score / total) * 100);

                quizResultCard.classList.remove('d-none');
                const scoreElem = document.getElementById('quizScoreText');
                if (scoreElem) {
                    scoreElem.textContent = `${score}/${total} (${percent}%)`;
                }

                const verdictBadge = document.getElementById('quizVerdictBadge');
                if (verdictBadge) {
                    if (percent >= 66) {
                        verdictBadge.textContent = 'PASSED & CERTIFIED';
                        verdictBadge.className = 'badge badge-gold';
                    } else {
                        verdictBadge.textContent = 'REVIEW REQUIRED';
                        verdictBadge.className = 'badge badge-slate';
                    }
                }

                window.scrollTo({ top: quizResultCard.offsetTop - 80, behavior: 'smooth' });

                if (typeof LumoraToast === 'function') {
                    LumoraToast(`Quiz graded: ${score}/${total} Correct (${percent}%)`);
                }
            });
        }
    });
})();
