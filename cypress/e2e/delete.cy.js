describe('Delete Reservation', () => {
    it('should delete a reservation', () => {

        cy.visit('localhost:7199/Reservations/');
        cy.get('input[name="Email"]').type('przykladowymail@wp.pl');
        cy.get('input[name="Password"]').type('Haslo123');
    
        cy.contains('Zaloguj!').click();
        cy.visit('localhost:7199/Reservations/');
        cy.contains('Usuń!').click();
        cy.get('form').should('exist');


        cy.get('input[type="submit"]').should('exist');


        cy.contains('Usuń!').click();


    });
});