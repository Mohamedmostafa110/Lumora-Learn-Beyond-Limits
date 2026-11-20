/**
 * ============================================================================
 * LUMORA — Learn Beyond Limits | Enterprise Design System
 * File: charts-init.js
 * Purpose: Chart.js configurations for Instructor and Admin dashboards.
 *          STRICT COLOR POLICY: Midnight Navy, Champagne Gold, Slate Blue ONLY.
 *          ZERO GREEN / ZERO PURPLE.
 * Compatible with: ASP.NET Core MVC (Lumora.Web / wwwroot/js)
 * ============================================================================
 */

(function () {
  'use strict';

  document.addEventListener('DOMContentLoaded', function () {
    if (typeof Chart === 'undefined') return;

    // Global Chart.js Font & Color Defaults
    Chart.defaults.font.family = "'Plus Jakarta Sans', -apple-system, sans-serif";
    Chart.defaults.color = '#71849A';
    Chart.defaults.scale.grid.color = 'rgba(113, 132, 154, 0.12)';

    // ------------------------------------------------------------------------
    // 1. INSTRUCTOR DASHBOARD: Monthly Revenue & Enrollments Line Chart
    // ------------------------------------------------------------------------
    const instructorRevenueCanvas = document.getElementById('instructorRevenueChart');
    if (instructorRevenueCanvas) {
      new Chart(instructorRevenueCanvas, {
        type: 'line',
        data: {
          labels: ['Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov'],
          datasets: [
            {
              label: 'Revenue ($USD)',
              data: [6400, 7800, 9200, 8900, 11400, 14250],
              borderColor: '#D6B779', /* Champagne Gold */
              backgroundColor: 'rgba(214, 183, 121, 0.12)',
              borderWidth: 3,
              fill: true,
              tension: 0.35,
              pointBackgroundColor: '#D6B779',
              pointBorderColor: '#14243A',
              pointRadius: 4,
              yAxisID: 'y'
            },
            {
              label: 'Enrollments',
              data: [42, 54, 68, 62, 82, 108],
              borderColor: '#14243A', /* Midnight Navy */
              backgroundColor: 'transparent',
              borderWidth: 2,
              borderDash: [5, 5],
              tension: 0.3,
              pointBackgroundColor: '#14243A',
              pointRadius: 3,
              yAxisID: 'y1'
            }
          ]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          interaction: { mode: 'index', intersect: false },
          plugins: {
            legend: {
              position: 'top',
              labels: {
                boxWidth: 14,
                font: { weight: '600' }
              }
            },
            tooltip: {
              backgroundColor: '#14243A',
              titleColor: '#D6B779',
              bodyColor: '#FFFFFF',
              borderColor: 'rgba(214, 183, 121, 0.3)',
              borderWidth: 1,
              padding: 10
            }
          },
          scales: {
            y: {
              type: 'linear',
              display: true,
              position: 'left',
              ticks: {
                callback: function (value) { return '$' + value; }
              }
            },
            y1: {
              type: 'linear',
              display: true,
              position: 'right',
              grid: { drawOnChartArea: false }
            }
          }
        }
      });
    }

    // ------------------------------------------------------------------------
    // 2. ADMIN DASHBOARD: Platform Gross Platform Growth (Line Chart)
    // ------------------------------------------------------------------------
    const adminGrowthCanvas = document.getElementById('adminPlatformGrowthChart');
    if (adminGrowthCanvas) {
      new Chart(adminGrowthCanvas, {
        type: 'line',
        data: {
          labels: ['May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov'],
          datasets: [
            {
              label: 'Active Enterprise Learners',
              data: [18000, 24000, 31000, 37500, 43000, 48200, 52400],
              borderColor: '#14243A',
              backgroundColor: 'rgba(20, 36, 58, 0.08)',
              borderWidth: 3,
              fill: true,
              tension: 0.35,
              pointBackgroundColor: '#14243A',
              pointRadius: 4
            },
            {
              label: 'Verifiable Certs Issued',
              data: [1200, 1850, 2600, 3400, 4100, 4900, 5820],
              borderColor: '#D6B779',
              backgroundColor: 'rgba(214, 183, 121, 0.1)',
              borderWidth: 2.5,
              fill: true,
              tension: 0.35,
              pointBackgroundColor: '#D6B779',
              pointRadius: 4
            }
          ]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: { position: 'top', labels: { boxWidth: 14, font: { weight: '600' } } },
            tooltip: {
              backgroundColor: '#14243A',
              titleColor: '#D6B779',
              bodyColor: '#FFFFFF',
              borderColor: 'rgba(214, 183, 121, 0.3)',
              borderWidth: 1
            }
          }
        }
      });
    }

    // ------------------------------------------------------------------------
    // 3. ADMIN DASHBOARD: Users by Role Breakdown (Doughnut Chart)
    // ------------------------------------------------------------------------
    const adminUserDoughnutCanvas = document.getElementById('adminUserRoleDoughnut');
    if (adminUserDoughnutCanvas) {
      new Chart(adminUserDoughnutCanvas, {
        type: 'doughnut',
        data: {
          labels: ['Enterprise Students (94%)', 'Practitioner Faculty (4.5%)', 'Governance Admins (1.5%)'],
          datasets: [
            {
              data: [49250, 2350, 800],
              backgroundColor: [
                '#14243A', /* Midnight Navy */
                '#D6B779', /* Champagne Gold */
                '#71849A'  /* Slate Blue */
              ],
              borderColor: '#FFFFFF',
              borderWidth: 2,
              hoverOffset: 6
            }
          ]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: { position: 'bottom', labels: { boxWidth: 12, padding: 16 } }
          },
          cutout: '72%'
        }
      });
    }
  });
})();
